using System.Threading.Tasks;
using OAuth2.Models;
using OAuth2.Transfer;

namespace OAuth2.Contracts.Services.User
{
    public interface IUpdateUser<TUser> where TUser : User<TUser>
    {
        public Task<TUser> Update<TUpdateDto>(TUser user, TUpdateDto dto)
            where TUpdateDto : UpdateUser;
    }
}