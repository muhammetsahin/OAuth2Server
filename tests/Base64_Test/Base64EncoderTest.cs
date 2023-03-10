using System;
using System.Security.Cryptography;
using Xunit;
using Base64;
using FluentAssertions;

namespace Base64_Test
{
    public class Base64EncoderTest
    {
        [Fact]
        public void Should_Encode_Base64_In_Original_Mode()
        {
            byte[] bytes = new byte[64];
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

            provider.GetNonZeroBytes(bytes);
            var base64 = new Base64EncoderDecoder();

            string b64 = base64.Encode(bytes, Variant.Original);

            b64.Should()
                .NotBeEmpty()
                .And
                .BeEquivalentTo(Convert.ToBase64String(bytes));
        }

        [Fact]
        public void Should_Encode_Base64_In_UrlSafe_Mode()
        {
            byte[] bytes = new byte[64];
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

            provider.GetNonZeroBytes(bytes);
            var base64 = new Base64EncoderDecoder();

            string b64 = base64.Encode(bytes, Variant.UrlSafe);

            b64.Should()
                .NotBeEmpty()
                .And
                .BeEquivalentTo(
                    Convert.ToBase64String(bytes)
                        .Replace('/', '_')
                        .Replace('+', '-')
                );
        }

        [Fact]
        public void Should_Encode_Base64_In_Original_Mode_With_No_Padding()
        {
            byte[] bytes = new byte[64];
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

            provider.GetNonZeroBytes(bytes);
            var base64 = new Base64EncoderDecoder();

            string b64 = base64.Encode(bytes, Variant.OriginalNoPadding);

            b64.Should()
                .NotBeEmpty()
                .And
                .BeEquivalentTo(Convert.ToBase64String(bytes).TrimEnd('='));
        }

        [Fact]
        public void Should_Encode_Base64_In_UrlSafe_Mode_With_No_Padding()
        {
            byte[] bytes = new byte[64];
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

            provider.GetNonZeroBytes(bytes);
            var base64 = new Base64EncoderDecoder();

            string b64 = base64.Encode(bytes, Variant.UrlSafeNoPadding);

            b64.Should()
                .NotBeEmpty()
                .And
                .BeEquivalentTo(
                    Convert.ToBase64String(bytes)
                        .TrimEnd('=')
                        .Replace('/', '_')
                        .Replace('+', '-')
                );
        }
    }
}