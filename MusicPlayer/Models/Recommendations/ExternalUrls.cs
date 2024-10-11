using Newtonsoft.Json;

namespace MusicPlayer.Models.Recommendations
{
    public class ExternalUrls
    {
        [JsonProperty("spotify")]

        public string Spotify { get; set; }
    }
}
