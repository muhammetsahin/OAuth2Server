using System;
using System.Threading.Tasks;
using OAuth2.Contracts.Providers;
using OAuth2.Contracts.Services.Client;
using OAuth2.Models;
using OAuth2Server.Exceptions;
using Sodium;

namespace OAuth2Server.Providers
{
    public class DefaultClientProvider<TUser> : IClientProvider<TUser> where TUser : User<TUser>
    {
        private readonly IGetClient<TUser> _getClient;

        public DefaultClientProvider(IGetClient<TUser> getClient)
        {
            _getClient = getClient;
        }


        public async Task<Client<TUser>> Fetch(string clientId, byte[] clientSecret,
            Func<Client<TUser>, bool> additionalCheck = null)
        {
            var client = await _getClient.Get(clientId);

            byte[] clientSecretFromDbBytes = Utilities.Base64ToBinary(client.Secret, "", Utilities.Base64Variant.UrlSafeNoPadding);
    

            if (!Utilities.Compare(GenericHash.Hash(clientSecret, null, 64), clientSecretFromDbBytes))
            {
                throw new ClientSecretInvalidException();
            }

            if (additionalCheck != null && additionalCheck(client) == false)
            {
                throw new Exception("Additional checks on Client failed");
            }

            return client;
        }

        public Task<Client<TUser>> Fetch(string clientId, string clientSecret,
            Func<Client<TUser>, bool> additionalCheck = null)
        {
            byte[] clientSecretBytes =
                Utilities.Base64ToBinary(clientSecret, "", Utilities.Base64Variant.UrlSafeNoPadding);

            return Fetch(clientId, clientSecretBytes, additionalCheck);
        }
    }
}