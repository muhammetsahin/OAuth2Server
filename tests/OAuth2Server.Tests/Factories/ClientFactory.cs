using System;
using Bogus;
using NodaTime;
using OAuth2.Models;

namespace OAuth2Server.Tests.Factories
{
    public class ClientFactory : IFactory<Client<User>>
    {
        private static int _id = 1;
        
        public Faker<Client<User>> Create()
            => new Faker<Client<User>>()
                .RuleFor(c => c.Id, f => _id++)
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.CreatedAt, f => Instant.FromDateTimeUtc(DateTime.UtcNow))
                .RuleFor(c => c.UpdateAt, f => Instant.FromDateTimeUtc(DateTime.UtcNow))
                .RuleFor(c => c.Revoked, false);
    }
}