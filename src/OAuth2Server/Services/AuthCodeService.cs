using System;
using System.Linq;
using System.Threading.Tasks;
using Base64;
using OAuth2.Contracts.Services.AuthCode;
using OAuth2.Models;
using OAuth2.Transfer;
using Sodium;

namespace OAuth2Server.Services
{
    public class AuthCodeService<TUser> : IAuthCodeService<TUser> where TUser : User<TUser>
    {
        private readonly ICreateAuthCode<TUser> _createAuthCode;

        public AuthCodeService(ICreateAuthCode<TUser> createAuthCode)
        {
            _createAuthCode = createAuthCode;
        }

        public async Task<AuthCode<TUser>> Get(string code)
        {
            return null;
        }

        public async Task<AuthCode<TUser>> Get(Guid id)
        {
            return null;
        }

        public Task<AuthCode<TUser>> Store(IAuthorize authorize, Client<TUser> client, TUser user)
        {
            var base64 = new Base64EncoderDecoder();

            var code = base64.Encode(SodiumCore.GetRandomBytes(128), Variant.UrlSafeNoPadding);
            var scopes = authorize.Scopes.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            return _createAuthCode.Create(code, client.Id, user.Id, scopes);
        }

        public async Task<bool> Delete(string code)
        {
            
            
            return false;
        }

        public async Task<bool> Delete(long id)
        {
            return false;
        }

        public async Task<bool> DeleteExpired()
        {
            return false;
        }
    }
}