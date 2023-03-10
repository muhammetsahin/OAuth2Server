using System;
using Bogus;
using NodaTime;

namespace OAuth2Server.Tests.Factories
{
    public class UserFactory : IFactory<User>
    {
        private static int _id = 1;

        public Faker<User> Create() =>
            new Faker<User>()
                .RuleFor(u => u.Id, f => _id++)
                .RuleFor(u => u.Name, f => f.Name.FirstName())
                .RuleFor(u => u.Surname, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => "")
                .RuleFor(u => u.CreatedAt, f => Instant.FromDateTimeUtc(DateTime.UtcNow))
                .RuleFor(u => u.UpdateAt, f => Instant.FromDateTimeUtc(DateTime.UtcNow))
                .RuleFor(u => u.EmailVerifier, f => Instant.FromDateTimeUtc(DateTime.UtcNow));
    }
}