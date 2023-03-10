using System.Security.Cryptography;
using Base64;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Base64_Test
{
    public class Base64DecoderTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Base64DecoderTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ShouldDecodeOriginalString()
        {
            byte[] bytes = new byte[64];
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

            provider.GetNonZeroBytes(bytes);
            var base64 = new Base64EncoderDecoder();
            
            string b64 = base64.Encode(bytes, Variant.Original);

            _testOutputHelper.WriteLine(b64);

            byte[] buffer = base64.Decode(b64, Variant.Original)
                .ToArray();
            
            buffer.Should()
                .HaveCount(64)
                .And
                .Equal(bytes);
        }
    }
}