using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using OAuth2.Contracts.Services;
using OAuth2.Contracts.Services.Client;
using OAuth2.Models;
using OAuth2.Transfer;
using OAuth2Server.Services;
using OAuth2Server.Tests.Factories;
using Xunit;

namespace OAuth2Server.Tests.Services
{
    internal class CreateClient : ICreateClient<User>
    {
        public Task<Client<User>> Create(Client<User> client)
        {
            return Task.FromResult(client);
        }
    }

    public class ClientServiceTest
    {
        private readonly IFactory<User> _userFactory;
        private readonly ICreateClient<User> _createClient;
        private readonly IGetClient<User> _getClient;
        
        public ClientServiceTest()
        {
            _userFactory = new UserFactory();
            _createClient = new CreateClient();
            _getClient = Substitute.For<IGetClient<User>>();
        }

        [Fact]
        public async Task Should_Create_New_OAuth2_Client()
        {
            var user = _userFactory
                .Create()
                .Generate();
            var service = new ClientService<User>(_createClient, _getClient);

            var (client, clientId, clientSecret) = await service.Create(new Client
            {
                Name = "OAuth2 Client",
                Redirects = new List<string> {"http://localhost/redirect"},
                Scopes = new List<string> {"some-scope"},
                IsPasswordClient = true,
                IsPublicClient = true
            }, user, true);

            client
                .Should()
                .NotBeNull();

            clientId.Should()
                .NotBeEmpty()
                .And
                .MatchRegex("^[A-Za-z0-9_-]+$");

            clientSecret.Should()
                .NotBeEmpty()
                .And
                .MatchRegex("^[A-Za-z0-9_-]+$");
        }

        [Fact]
        public async Task Should_Throw_Exception_For_PasswordClient_On_Not_First_Party()
        {
            var user = _userFactory
                .Create()
                .Generate();
            var service = new ClientService<User>(_createClient, _getClient);

            await Assert.ThrowsAsync<Exception>(async () =>
                await service.Create(new Client
                {
                    Name = "OAuth2 Client",
                    Redirects = new List<string> {"http://localhost/redirect"},
                    Scopes = new List<string> {"some-scope"},
                    IsPasswordClient = true,
                    IsPublicClient = true
                }, user, false)
            );
        }
    }
}