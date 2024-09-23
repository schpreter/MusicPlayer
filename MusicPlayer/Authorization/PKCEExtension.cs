using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Authorization
{
    public static class PKCEExtension
    {
        public const string possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /*
         * Generate a random, cryptographically strong string of a specified length
         */
        public static string GenerateCodeVerifier(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++) 
            {
                sb.Append(possible[RandomNumberGenerator.GetInt32(possible.Length)]);
            }
            return Base64UrlTextEncoder.Encode(Encoding.UTF8.GetBytes(sb.ToString())); ;
        }

        private static byte[] GenerateSHA256(byte[] data)
        {
            return SHA256.HashData(data);
        }

        public static string GenerateCodeChallenge(byte[] data)
        {
            return Base64UrlTextEncoder.Encode(GenerateSHA256(data));
        }

    }
}
