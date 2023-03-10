using System.Security.Principal;
using System.Threading.Tasks;
using OAuth2.Models;

namespace OAuth2.Contracts.Services.User
{
    public interface IGetUser<TUser> where TUser : User<TUser>
    {
        Task<TUser> Get(string email);
        Task<TUser> Get(long id);
    }
}