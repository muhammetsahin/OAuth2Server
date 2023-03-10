using System;
using System.Collections.Generic;
using System.Text;

namespace Base64
{
    public class Base64EncoderDecoder : IEncoder, IDecoder
    {
        private static char Base64ByteToUrlSafeChar(int x) =>
            (char) (((x < 26 ? 0xFF : 0) & (x + 'A')) |
                    ((x >= 26 ? 0xFF : 0) & (x < 52 ? 0xFF : 0) & (x + ('a' - 26))) |
                    ((x >= 52 ? 0xFF : 0) & (x < 62 ? 0xFF : 0) & (x + ('0' - 52))) |
                    ((x == 62 ? 0xFF : 0) & '-') |
                    ((x == 63 ? 0xFF : 0) & '_'));

        private static char Base64ByteToChar(int x) =>
            (char) (((x < 26 ? 0xFF : 0) & (x + 'A')) |
                    ((x >= 26 ? 0xFF : 0) & (x < 52 ? 0xFF : 0) & (x + ('a' - 26))) |
                    ((x >= 52 ? 0xFF : 0) & (x < 62 ? 0xFF : 0) & (x + ('0' - 52))) |
                    ((x == 62 ? 0xFF : 0) & '+') |
                    ((x == 63 ? 0xFF : 0) & '/'));

        private static byte Base64CharToByte(char c, bool urlSafe)
        {
            int y = urlSafe ? '-' : '+';
            int y1 = urlSafe ? '_' : '/';
            int x =
                (((int) c >= (int) 'A' ? 0xFF : 0) & ((int) c <= (int) 'Z' ? 0xFF : 0) & (c - 'A')) |
                (((int) c >= (int) 'a' ? 0xFF : 0) & ((int) c <= (int) 'z' ? 0xFF : 0) & (c - ('a' - 26))) |
                (((int) c >= (int) '0' ? 0xFF : 0) & ((int) c <= (int) '9' ? 0xFF : 0) & (c - ('0' - 52))) |
                (((int) c == y ? 0xFF : 0) & 62) |
                (((int) c == y1 ? 0xFF : 0) & 63);


            return (byte) (x | ((x == 0 ? 0xFF : 0) & (((int) c == (int) 'A' ? 0xFF : 0) ^ 0xFF)));
        }


        /// <summary>
        /// Calculates the length of the base64 encoded string for the given buffer length
        /// </summary>
        /// <param name="bufferLength">Length of the buffer</param>
        /// <param name="variant">Base64 Variant</param>
        /// <returns>Length of the base64 string</returns>
        public int EncodedLength(int bufferLength, Variant variant)
        {
            if (((int) variant & (int) Mask.NoPadding) == 0)
            {
                return ((bufferLength + 2) / 3) << 2;
            }

            return ((bufferLength << 2) | 2) / 3;
        }

        /// <summary>
        /// Calculates number of bytes that will be used in byte buffer when decoding
        /// </summary>
        /// <param name="base64">Base64 encoded string</param>
        /// <param name="variant">Variant of base64 used in encoding</param>
        /// <returns>Number of bytes in original buffer</returns>
        public int EncodedLengthToBytes(ReadOnlySpan<char> base64, Variant variant)
        {
            if (((int) variant & (int) Mask.NoPadding) != 0)
            {
                return 3 * (base64.Length / 4);
            }

            int count = 0;

            foreach (var c in base64[^2..])
            {
                if (c == '=') count++;
            }

            return 3 * (base64.Length / 4) - count;
        }

        /// <summary>
        /// Encodes byte array into base64 string with 4 Variants
        /// 1. Standard with padding
        /// 2. Standard with no padding
        /// 3. UrlSafe with padding
        /// 4. UrlSafe with no padding
        /// </summary>
        /// <param name="bytes">Input buffer (byte array)</param>
        /// <param name="variant">Base64 Variant</param>
        /// <returns>Base64 Encoded string</returns>
        public string Encode(ReadOnlySpan<byte> bytes, Variant variant)
        {
            int b64MaxLen = EncodedLength(bytes.Length, variant);
            StringBuilder builder = new StringBuilder(b64MaxLen);

            int accLen = 0;
            int nibbles = bytes.Length / 3;
            int remainder = bytes.Length - 3 * nibbles;
            int b64Pos = 0;
            int binPos = 0;
            int acc = 0;
            int b64Len = nibbles * 4;

            if (remainder != 0)
            {
                // With Padding
                if (((int) variant & (int) Mask.NoPadding) == 0)
                {
                    b64Len += 4;
                }
                // With no padding
                else
                {
                    b64Len += 2 + (remainder >> 1);
                }
            }

            // URL Safe variant
            if (((int) variant & (int) Mask.UrlSafe) != 0)
            {
                while (binPos < bytes.Length)
                {
                    acc = (acc << 8) + bytes[binPos++];
                    accLen += 8;
                    while (accLen >= 6)
                    {
                        accLen -= 6;
                        builder.Insert(b64Pos++, Base64ByteToUrlSafeChar((acc >> accLen) & 0x3F));
                    }
                }

                if (accLen > 0)
                {
                    builder.Insert(b64Pos++, Base64ByteToUrlSafeChar((acc << (6 - accLen)) & 0x3F));
                }
            }
            // Standard option
            else
            {
                while (binPos < bytes.Length)
                {
                    acc = (acc << 8) + bytes[binPos++];
                    accLen += 8;
                    while (accLen >= 6)
                    {
                        accLen -= 6;
                        builder.Insert(b64Pos++, Base64ByteToChar((acc >> accLen) & 0x3F));
                    }
                }

                if (accLen > 0)
                {
                    builder.Insert(b64Pos++, Base64ByteToChar((acc << (6 - accLen)) & 0x3F));
                }
            }

            while (b64Pos < b64Len)
            {
                builder.Insert(b64Pos++, '=');
            }


            return builder.ToString();
        }


        /// <summary>
        /// Decodes base64 string into buffer (byte array) with variants
        /// 1. Standard with padding
        /// 2. Standard with no padding
        /// 3. UrlSafe with padding
        /// 4. UrlSafe with no padding
        /// </summary>
        /// <param name="base64">Base64 Encoded string</param>
        /// <param name="variant">Variant used in encoding</param>
        /// <param name="ignore">Characters to ignore</param>
        /// <returns>Return Memory&gt;byte&lt; with underlying byte buffer or null on error</returns>
        public Memory<byte> Decode(ReadOnlySpan<char> base64, Variant variant, HashSet<char> ignore = null)
        {
            byte[] data = new byte[EncodedLengthToBytes(base64, variant)];
            int accLen = 0, b64Pos = 0, binPos = 0, acc = 0;
            bool isUrlSafe = ((int) variant & (int) Mask.UrlSafe) == 1;


            while (b64Pos < base64.Length)
            {
                char c = base64[b64Pos];
                byte d = Base64CharToByte(c, isUrlSafe);

                if (d == 0xFF)
                {
                    // Check if this character is in ignore list
                    if (ignore != null && ignore.Contains(c))
                    {
                        b64Pos++;
                        continue;
                    }

                    break;
                }

                acc = (acc << 6) + d;
                accLen += 6;
                if (accLen >= 8)
                {
                    accLen -= 8;

                    // This really should never happen
                    if (binPos >= data.Length)
                    {
                        throw new OverflowException();
                    }

                    data[binPos++] = (byte) ((acc >> accLen) & 0xFF);
                }

                b64Pos++;
            }

            // Check for padding
            if (((int) variant & (int) Mask.NoPadding) == 0)
            {
                int paddingLen = accLen / 2;

                while (paddingLen > 0)
                {
                    if (b64Pos >= base64.Length)
                    {
                        return null;
                    }

                    char c = base64[b64Pos];

                    if (c == '=')
                    {
                        paddingLen--;
                    }
                    else if (ignore == null || ignore.Contains(c))
                    {
                        return null;
                    }

                    b64Pos++;
                }
            }
            else if (ignore != null)
            {
                while (b64Pos < base64.Length && ignore.Contains(base64[b64Pos]))
                {
                    b64Pos++;
                }
            }
            else if (b64Pos != base64.Length)
            {
                return null;
            }

            return data;
        }
    }
}