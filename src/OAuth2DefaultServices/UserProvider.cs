using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OAuth2.Contracts.Providers;
using OAuth2.Models;
using OAuth2EntityCore;

namespace OAuth2DefaultServices
{
    public class UserProvider<TUser> : IUserProvider<TUser> where TUser : User<TUser>
    {
        private readonly IOAuth2DbContext<TUser> _context;

        public UserProvider(IOAuth2DbContext<TUser> context)
        {
            _context = context;
        }

        public Task<TUser> Fetch(string userNameOrEmail)
        {
            return _context.Users.Where(u => u.Email == userNameOrEmail).SingleAsync();
        }
    }
}