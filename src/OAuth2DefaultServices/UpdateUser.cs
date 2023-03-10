using System;
using System.Threading.Tasks;
using NodaTime;
using OAuth2.Contracts.Services.User;
using OAuth2.Models;
using OAuth2EntityCore;

namespace OAuth2DefaultServices
{
    public class UpdateUser<TUser> : IUpdateUser<TUser> where TUser : User<TUser>
    {
        private readonly IOAuth2DbContext<TUser> _context;

        public UpdateUser(IOAuth2DbContext<TUser> context)
        {
            _context = context;
        }

        public async Task<TUser> Update<TUpdateDto>(TUser user, TUpdateDto dto)
            where TUpdateDto : OAuth2.Transfer.UpdateUser
        {
            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;
            user.Password = dto.Password;
            user.EmailVerifier = dto.EmailVerifier;
            user.UpdateAt = Instant.FromDateTimeUtc(DateTime.UtcNow);
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}