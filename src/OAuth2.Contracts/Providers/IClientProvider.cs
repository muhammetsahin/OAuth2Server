using System;
using System.Threading.Tasks;
using OAuth2.Models;

namespace OAuth2.Contracts.Providers
{
    public interface IClientProvider<TUser> where TUser : User<TUser>
    {
        public Task<Client<TUser>> Fetch(string clientId, byte[] clientSecret,
            Func<Client<TUser>, bool> additionalCheck = null);

        Task<Client<TUser>> Fetch(string clientId, string clientSecret, Func<Client<TUser>, bool> additionalCheck = null);
    }
}