using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OAuth2.Contracts.Services.AuthCode;
using OAuth2.Models;
using OAuth2EntityCore;

namespace OAuth2DefaultServices
{
    public class DeleteAuthCode<TUser> : IDeleteAuthCode<TUser> where TUser : User<TUser>
    {
        private readonly IOAuth2DbContext<TUser> _context;

        public DeleteAuthCode(IOAuth2DbContext<TUser> context)
        {
            _context = context;
        }

        public async Task<bool> Delete(string code)
        {
            var authCode = await _context.AuthCodes
                .Where(a => a.Code == code)
                .SingleOrDefaultAsync();
            _context.AuthCodes.Remove(authCode);

            var count = await _context.SaveChangesAsync();
            
            return count > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            _context.AuthCodes.Remove(new AuthCode<TUser>{ Id = id });
            
            var count = await _context.SaveChangesAsync();
            
            return count > 0;
        }

        public Task<bool> DeleteExpired()
        {
            throw new System.NotImplementedException();
        }
    }
}