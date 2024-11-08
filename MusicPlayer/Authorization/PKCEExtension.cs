using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace MusicPlayer.Authorization
{
    public static class PKCEExtension
    {
        public const string possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /*
         * Generate a random, cryptographically strong base64url encoded string of a specified length.
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

        /*
         * Base64url encodes the data.
         */
        public static string GenerateCodeChallenge(byte[] data)
        {
            return Base64UrlTextEncoder.Encode(SHA256.HashData(data));
        }

    }
}
