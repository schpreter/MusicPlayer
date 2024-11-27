using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace MusicPlayer.Authorization
{
    /// <summary>
    /// Static class, containg methods responsible for the PKCE flow (<see href="https://oauth.net/2/pkce/"/>)
    /// </summary>
    public static class PKCEExtension
    {
        public const string possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// Generate a random, cryptographically strong base64url encoded string of a specified length.
        /// </summary>
        /// <param name="length">The length of the string we wish to generate</param>
        /// <returns></returns>
        public static string GenerateCodeVerifier(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(possible[RandomNumberGenerator.GetInt32(possible.Length)]);
            }
            return Base64UrlTextEncoder.Encode(Encoding.UTF8.GetBytes(sb.ToString())); ;
        }

        /// <summary>
        /// Base64url encodes the supplied data.
        /// </summary>
        /// <param name="data">The data we wish to encode in a byte array format</param>
        /// <returns></returns>
        public static string GenerateCodeChallenge(byte[] data)
        {
            return Base64UrlTextEncoder.Encode(SHA256.HashData(data));
        }

    }
}
