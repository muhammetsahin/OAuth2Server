using System;
using System.Threading.Tasks;
using OAuth2.Models;

namespace OAuth2.Contracts.Services.AuthCode
{
    public interface IDeleteAuthCode<TUser> where TUser : User<TUser>
    {
        Task<bool> Delete(string code);
        Task<bool> Delete(Guid id);
        Task<bool> DeleteExpired();
    }
}