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
        public static string GenerateRandomString(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++) 
            {
                sb.Append(possible[RandomNumberGenerator.GetInt32(possible.Length)]);
            }
            return sb.ToString();
        }

        public static string GenerateCodeChallenge(string rawInput)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(rawInput);
            return Convert.ToBase64String(sha256.ComputeHash(bytes));
        }

    }
}
