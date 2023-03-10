using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OAuth2.Contracts.Providers;
using OAuth2.Contracts.Services.User;
using OAuth2.Models;
using OAuth2EntityCore;

namespace OAuth2DefaultServices
{
    public class GetUser<TUser> : IProvider<ClaimsPrincipal, TUser>, IGetUser<TUser> where TUser : User<TUser>
    {
        private readonly IOAuth2DbContext<TUser> _context;

        public GetUser(IOAuth2DbContext<TUser> context)
        {
            _context = context;
        }

        public Task<TUser> Fetch(ClaimsPrincipal principal)
        {
            var id = principal.FindFirst(c => c.Type == ClaimTypes.Sid);

            if (id != null)
            {
                return _context.Users.FindAsync(long.Parse(id.Value)).AsTask();
            }

            var email = principal.FindFirst(c => c.Type == ClaimTypes.Email);

            if (email != null)
            {
                return _context.Users.Where(u => u.Email == email.Value).SingleOrDefaultAsync();
            }
            
            // TODO: Create custom exception
            throw new Exception("Email claim is not found");
        }
        
        public Task<TUser> Get(string email) => _context.Users.Where(u => u.Email == email).SingleOrDefaultAsync();

        public Task<TUser> Get(long id) => _context.Users.FindAsync(id).AsTask();
    }
}