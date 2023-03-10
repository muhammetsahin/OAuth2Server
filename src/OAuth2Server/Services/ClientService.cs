using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base64;
using NodaTime;
using OAuth2.Contracts.Services.Client;
using OAuth2.Models;
using Sodium;

namespace OAuth2Server.Services
{
    public class ClientService<TUser> : IClientService<TUser>
        where TUser : User<TUser>
    {
        private readonly ICreateClient<TUser> _createClient;
        private readonly IGetClient<TUser> _getClient;

        public ClientService(ICreateClient<TUser> createClient, IGetClient<TUser> getClient)
        {
            _createClient = createClient;
            _getClient = getClient;
        }

        public Task<List<Client<TUser>>> Get(int page, int perPage)
        {
            return _getClient.Get(page, perPage);
        }

        public Task<Client<TUser>> Get(long id)
        {
            return _getClient.Get(id);
        }

        /// <summary>
        /// Creates new OAuth2 client and generates client_id and client_secret
        /// Both of those values should be cryptographically secure random bytes and
        /// encoded with base64url encoding (to support use in urls)
        ///
        /// For generation of these 2 items (client_id and client_secret) libsodium is used
        /// </summary>
        /// <param name="dto">Transfer object containing client data</param>
        /// <param name="user">Current user that initiated a method call</param>
        /// <param name="isFirstParty"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(Client<TUser> client, string clientId, string clientSecret)> Create(
            OAuth2.Transfer.Client dto, TUser user, bool isFirstParty)
        {
            var base64 = new Base64EncoderDecoder();
            string clientId = base64.Encode(SodiumCore.GetRandomBytes(16), Variant.UrlSafeNoPadding);

            // Client secret should be at least 32 bytes long
            // longer the better (64 is good number)
            byte[] clientSecret = SodiumCore.GetRandomBytes(64);

            if (!isFirstParty && dto.IsPasswordClient)
            {
                throw new Exception("Cannot use password client on non first party apps");
            }

            var client = await _createClient.Create(new Client<TUser>
            {
                Name = dto.Name,
                Redirects = dto.Redirects,
                Scopes = dto.Scopes,
                Revoked = false,
                // If you really want to protect client secret, then treat it as password (Hash and salt) with good
                // password
                // hashing functions (Argon2) (SCrypt | BCrypt - less preferable)
                Secret = base64.Encode(GenericHash.Hash(clientSecret, null, 64), Variant.UrlSafeNoPadding),
                ClientId = clientId,
                IsFirstParty = isFirstParty,
                IsPasswordClient = dto.IsPasswordClient,
                IsPublicClient = dto.IsPublicClient,
                CreatedAt = Instant.FromDateTimeUtc(DateTime.UtcNow),
                UpdateAt = Instant.FromDateTimeUtc(DateTime.UtcNow),
                UserId = user.Id,
            });

            return (client, clientId, base64.Encode(clientSecret, Variant.UrlSafeNoPadding));
        }

        public Task Update(long id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }
    }
}