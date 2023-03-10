using System;

namespace Base64
{
    public interface IEncoder
    {
        /// <summary>
        /// Calculates the length of the base64 encoded string for the given buffer length
        /// </summary>
        /// <param name="bufferLength">Length of the buffer</param>
        /// <param name="variant">Base64 Variant</param>
        /// <returns>Length of the base64 string</returns>
        int EncodedLength(int bufferLength, Variant variant);

        /// <summary>
        /// Encodes byte array into base64 string with 4 Variants
        /// 1. Standard with padding
        /// 2. Standard with no padding
        /// 3. UrlSafe with padding
        /// 4. UrlSafe with no padding
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="variant"></param>
        /// <returns>Base64 Encoded string</returns>
        public string Encode(ReadOnlySpan<byte> bytes, Variant variant);
    }
}