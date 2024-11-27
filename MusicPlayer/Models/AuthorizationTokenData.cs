using Newtonsoft.Json;

namespace MusicPlayer.Models
{    /// <summary>
     /// Class structure storing the authorization response token.
     /// </summary>
    public class AuthorizationTokenData
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        public AuthorizationTokenData()
        {

        }
    }
}
