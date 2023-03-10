using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using OAuth2.Contracts.Providers;
using OAuth2.Contracts.Services.Client;
using OAuth2.Models;
using OAuth2Server.Exceptions;
using OAuth2Server.Providers;
using Sodium;
using Xunit;

namespace OAuth2Server.Tests.Providers
{
    public class DefaultClientProviderTest
    {
        private readonly byte[] _secret;
        private readonly Client<User> _client;
        private readonly IGetClient<User> _getClient;
        private readonly string _secretAsString;

        public DefaultClientProviderTest()
        {
            _secret = SodiumCore.GetRandomBytes(64);

            _client = new Client<User>
            {
                Id = 1L,
                Name = "Test OAuth2 Client",
                Redirects = new List<string> {"http://localhost/redirect"},
                Revoked = false,
                Scopes = new List<string> {"*"},
                IsFirstParty = true,
                IsPasswordClient = true,
                ClientId = Utilities.BinaryToBase64(SodiumCore.GetRandomBytes(16),
                    Utilities.Base64Variant.UrlSafeNoPadding),
                Secret = Utilities.BinaryToBase64(GenericHash.Hash(_secret, null, 64),
                    Utilities.Base64Variant.UrlSafeNoPadding),
                IsPublicClient = true,
            };

            _getClient = Substitute.For<IGetClient<User>>();
            _getClient.Get(_client.ClientId).Returns(_client);
            _secretAsString = Utilities.BinaryToBase64(_secret, Utilities.Base64Variant.UrlSafeNoPadding);
        }


        [Fact]
        public async Task Should_Return_OAuth2_Client()
        {
            IClientProvider<User> provider = new DefaultClientProvider<User>(_getClient);

            var fetchedClient = await provider.Fetch(_client.ClientId, _secret, c => true);

            fetchedClient.Id.Should()
                .Be(_client.Id);

            fetchedClient.Name.Should().BeEquivalentTo("Test OAuth2 Client");
        }

        [Fact]
        public async Task Should_Return_OAuth2Client_With_Secret_As_String()
        {
            IClientProvider<User> provider = new DefaultClientProvider<User>(_getClient);

            var fetchedClient = await provider.Fetch(_client.ClientId, _secretAsString, c => true);

            fetchedClient.Id.Should()
                .Be(_client.Id);

            fetchedClient.Name
                .Should()
                .BeEquivalentTo("Test OAuth2 Client");
        }


        [Fact]
        public async Task Should_Throw_Exception_For_Invalid_Secret()
        {
            await Assert.ThrowsAsync<ClientSecretInvalidException>(async () =>
            {
                IClientProvider<User> provider = new DefaultClientProvider<User>(_getClient);
                byte[] secret = SodiumCore.GetRandomBytes(64);

                await provider.Fetch(_client.ClientId, secret, c => true);
            });
        }

        [Fact]
        public async Task Should_Throw_Exception_For_Invalid_Secret_As_String()
        {
            await Assert.ThrowsAsync<ClientSecretInvalidException>(async () =>
            {
                IClientProvider<User> provider = new DefaultClientProvider<User>(_getClient);
                string secret = Utilities.BinaryToBase64(SodiumCore.GetRandomBytes(64),
                    Utilities.Base64Variant.UrlSafeNoPadding);
                await provider.Fetch(_client.ClientId, secret, c => true);
            });
        }
        
        [Fact]
        public async Task Should_Throw_Exception_For_Invalid_Base64Encoded_Secret()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                IClientProvider<User> provider = new DefaultClientProvider<User>(_getClient);
                string secret = Utilities.BinaryToBase64(SodiumCore.GetRandomBytes(64),
                    Utilities.Base64Variant.OriginalNoPadding);
                await provider.Fetch(_client.ClientId, secret, c => true);
            });
        }
        
    }
}