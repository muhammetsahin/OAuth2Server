using System;
using System.Collections.Generic;

namespace Base64
{
    public interface IDecoder
    {
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
        /// <returns>Return Memory&gt;byte&lt; with underlying byte buffer</returns>
        public Memory<byte> Decode(ReadOnlySpan<char> base64, Variant variant, HashSet<char> ignore = null);

        /// <summary>
        /// Calculates number of bytes that will be used in byte buffer when decoding
        /// </summary>
        /// <param name="base64">Base64 encoded string</param>
        /// <param name="variant">Variant of base64 used in encoding</param>
        /// <returns>Number of bytes in original buffer</returns>
        public int EncodedLengthToBytes(ReadOnlySpan<char> base64, Variant variant);
    }
}