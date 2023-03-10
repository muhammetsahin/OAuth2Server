using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NodaTime;
using OAuth2.Contracts.Services.AuthCode;
using OAuth2.Models;
using OAuth2EntityCore;

namespace OAuth2DefaultServices
{
    public class CreateAuthCode<TUser> : ICreateAuthCode<TUser> where TUser : User<TUser>
    {
        private readonly IOAuth2DbContext<TUser> _context;

        public CreateAuthCode(IOAuth2DbContext<TUser> context)
        {
            _context = context;
        }

        public async Task<AuthCode<TUser>> Create(string code, long clientId, long userId, List<string> scopes)
        {
            var authCode = new AuthCode<TUser>
            {
                ClientId = clientId,
                UserId = userId,
                Scopes = scopes,
                CreatedAt = Instant.FromDateTimeUtc(DateTime.UtcNow),
                Code = code,
                ExpiresAt = Instant.FromDateTimeUtc(DateTime.UtcNow).Plus(Duration.FromMinutes(15)),
                UpdateAt = Instant.FromDateTimeUtc(DateTime.UtcNow)
            };

            await _context.AuthCodes.AddAsync(authCode);

            await _context.SaveChangesAsync();

            return authCode;
        }
    }
}