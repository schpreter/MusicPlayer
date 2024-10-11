using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicPlayer.Models.Recommendations
{
    public class Tracks
    {
        [JsonProperty("album")]
        public Album Album { get; set; }
        [JsonProperty("artists")]

        public List<Artist> Artists { get; set; }
        [JsonProperty("avaliable_markets")]
        public List<string> AvaliableMarkets { get; set; }
        [JsonProperty("disc_number")]

        public int DiscNumber { get; set; }
        [JsonProperty("duration_ms")]

        public int DurationMs { get; set; }
        [JsonProperty("explicit")]

        public bool Explicit { get; set; }
        [JsonProperty("external_ids")]

        public ExternalIds ExternalIds { get; set; }
        [JsonProperty("external_urls")]

        public ExternalUrls ExternalUrls { get; set; }
        [JsonProperty("href")]

        public string Href { get; set; }
        [JsonProperty("id")]

        public string Id { get; set; }
        [JsonProperty("restrictions")]


        public Restrictions Restrictions { get; set; }
        [JsonProperty("name")]

        public string Name { get; set; }
        [JsonProperty("popularity")]

        public int Popularity { get; set; }
        [JsonProperty("track_number")]

        public int TrackNumber { get; set; }
        [JsonProperty("uri")]

        public string Uri { get; set; }

    }
}
