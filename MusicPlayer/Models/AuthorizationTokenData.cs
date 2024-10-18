using Newtonsoft.Json;

namespace MusicPlayer.Models
{
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
        public string RefreshToken { get; set; } = "AQCbTPpITqWmX75viK9qLwjch71tnL1gGhcvODOdT0dT5hWyjo86Prn_A6ciICAAJn9CMB0CDkyWpOcY0bR-QVDnZqmfJHGF2fpoAsJ634hw9Tm2DCY04hKoYX_OTVBreDQ";
        public AuthorizationTokenData()
        {

        }
    }
}
