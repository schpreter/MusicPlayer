using Newtonsoft.Json;

namespace MusicPlayer.Models.Recommendations
{
    public class ExternalIds
    {
        [JsonProperty("isrc")]

        public string Isrc { get; set; }
        [JsonProperty("ean")]

        public string Ean { get; set; }
        [JsonProperty("upc")]

        public string Upc { get; set; }
    }
}
