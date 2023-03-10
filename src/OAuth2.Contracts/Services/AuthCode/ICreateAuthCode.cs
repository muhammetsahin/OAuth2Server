using System.Collections.Generic;
using System.Threading.Tasks;
using OAuth2.Models;

namespace OAuth2.Contracts.Services.AuthCode
{
    public interface ICreateAuthCode<TUser> where TUser : User<TUser>
    {
        Task<AuthCode<TUser>> Create(string code, long clientId, long userId, List<string> scopes);
    }
}