using Newtonsoft.Json;

namespace MusicPlayer.Models.Recommendations
{
    public class Artist
    {
        [JsonProperty("external_urls")]

        public ExternalUrls ExternalUrls { get; set; }
        [JsonProperty("href")]
        public string Href { get; set; }
        [JsonProperty("id")]

        public string Id { get; set; }
        [JsonProperty("name")]

        public string Name { get; set; }
        [JsonProperty("uri")]

        public string Uri { get; set; }
    }
}
