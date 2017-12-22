using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BinanceClient
{
    public static class BinanceHelpers
    {
        private static readonly Encoding SignatureEncoding = Encoding.UTF8;

        public static string CreateSignature(this string message, string secret)
        {

            byte[] keyBytes = SignatureEncoding.GetBytes(secret);
            byte[] messageBytes = SignatureEncoding.GetBytes(message);
            HMACSHA256 hmacsha256 = new HMACSHA256(keyBytes);

            byte[] bytes = hmacsha256.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

    }
}
