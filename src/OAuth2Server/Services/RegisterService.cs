using System.Threading.Tasks;
using OAuth2.Contracts.Services.User;
using OAuth2.Models;
using OAuth2.Transfer;
using FluentValidation;
using Mediator;
using OAuth2.Events.Notifications;

namespace OAuth2Server.Services
{
    public class RegisterService<TUser> : IRegisterService<TUser>
        where TUser : User<TUser>, new()
    {
        private readonly IValidator<Register> _validator;
        private readonly IMediator _mediator;
        private readonly ICreateUser<TUser> _createUser;

        public RegisterService(
            IValidator<Register> validator,
            IMediator mediator,
            ICreateUser<TUser> createUser
        )
        {
            _validator = validator;
            _mediator = mediator;
            _createUser = createUser;
        }


        /// <summary>
        /// Registers new user and saves it to underlying storage
        /// </summary>
        /// <param name="dto"></param>
        /// <typeparam name="TRegister"></typeparam>
        /// <returns>Saved user</returns>
        public async Task<TUser> Register<TRegister>(TRegister dto)
            where TRegister : Register
        {
            await _validator.ValidateAndThrowAsync(dto);

            var user = await _createUser.Create(dto);

            await _mediator
                .SetPublishStrategy(PublishStrategy.ParallelNoWait)
                .Publish(new UserRegistered<TUser> {User = user});

            return user;
        }
    }
}