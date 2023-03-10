using System;
using System.Threading.Tasks;
using NodaTime;
using OAuth2.Contracts.Providers;
using OAuth2.Contracts.Services.User;
using OAuth2.Models;
using OAuth2.Transfer;
using OAuth2EntityCore;

namespace OAuth2DefaultServices
{
    public class CreateUser<TUser> : ICreateUser<TUser> where TUser : User<TUser>, new()
    {
        private readonly IOAuth2DbContext<TUser> _context;
        private readonly IPasswordProvider _passwordProvider;

        public CreateUser(IOAuth2DbContext<TUser> context, IPasswordProvider passwordProvider)
        {
            _context = context;
            _passwordProvider = passwordProvider;
        }

        public async Task<TUser> Create<TRegister>(TRegister register) where TRegister : Register
        {
            var password = _passwordProvider.Hash(register.Password).TrimEnd('\0');
            Instant now = Instant.FromDateTimeUtc(DateTime.UtcNow);
            var user = new TUser
            {
                Name = register.Name,
                Surname = register.Surname,
                Email = register.Email,
                Password = password,
                EmailVerifier = null,
                CreatedAt = now,
                UpdateAt = now
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}