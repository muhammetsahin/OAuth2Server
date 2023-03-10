using System;
using System.Threading.Tasks;
using OAuth2.Models;
using OAuth2.Transfer;

namespace OAuth2.Contracts.Services.AuthCode
{
    public interface IAuthCodeService<TUser> where TUser : User<TUser>
    {
        Task<AuthCode<TUser>> Get(Guid id);
        Task<AuthCode<TUser>> Get(string code);
        Task<AuthCode<TUser>> Store(IAuthorize authorize, Client<TUser> client, TUser user);
        Task<bool> Delete(string code);
        Task<bool> Delete(long id);
        Task<bool> DeleteExpired();
    }
}