using System.Threading.Tasks;
using Mediator;
using OAuth2.Contracts.Providers;
using OAuth2.Contracts.Services.User;
using FluentValidation;
using OAuth2.Events.Notifications;
using OAuth2.Models;
using OAuth2.Transfer;
using OAuth2Server.Exceptions;

namespace OAuth2Server.Services
{
    public class LoginService<TUser> : ILoginService<TUser> where TUser : User<TUser>
    {
        private readonly IUserProvider<TUser> _provider;
        private readonly IUpdateUser<TUser> _updateUser;
        private readonly IClientProvider<TUser> _clientProvider;
        private readonly IPasswordProvider _passwordProvider;
        private readonly IValidator<Login> _loginValidator;
        private readonly IMediator _mediator;

        public LoginService(
            IUserProvider<TUser> provider,
            IUpdateUser<TUser> updateUser,
            IClientProvider<TUser> clientProvider,
            IPasswordProvider passwordProvider,
            IValidator<Login> loginValidator,
            IMediator mediator
        )
        {
            _provider = provider;
            _updateUser = updateUser;
            _clientProvider = clientProvider;
            _passwordProvider = passwordProvider;
            _loginValidator = loginValidator;
            _mediator = mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCredentialsException"></exception>
        /// <exception cref="EmailIsNotVerifiedException"></exception>
        public async Task<TUser> Login(Login dto)
        {
            await _loginValidator.ValidateAndThrowAsync(dto);

            var client = await _clientProvider.Fetch(dto.ClientId, dto.ClientSecret,
                c => c.IsFirstParty && c.IsPasswordClient);

            await _mediator
                .Publish(new AttemptingAuthenticationNotification<TUser> {Client = client, Data = dto});

            var user = await _provider.Fetch(dto.Email);

            if (!_passwordProvider.Verify(dto.Password, user.Password))
            {
                await _mediator
                    .Publish(new InvalidUserCredentialsNotification<TUser> {User = user});
                throw new InvalidCredentialsException();
            }

            if (_passwordProvider.NeedsRehash(user.Password))
            {
                await _updateUser.Update(user, new UpdateUser
                {
                    Email = user.Email,
                    Name = user.Name,
                    Surname = user.Surname,
                    Password = _passwordProvider.Hash(dto.Password),
                    EmailVerifier = user.EmailVerifier
                });
            }

            if(user is IEmailVerified verified && verified.ShouldVerifyEmail() && !verified.IsEmailVerified()) {
                await _mediator
                    .Publish(new EmailNotVerifiedNotification<TUser>
                    {
                        User = user
                    });
            
                throw new EmailIsNotVerifiedException();           
            }

            await _mediator
                .Publish(new LoginSuccessfulNotification<TUser> {Client = client, User = user});
            
            return user;
        }
    }
}