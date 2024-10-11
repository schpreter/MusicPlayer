using Newtonsoft.Json;

namespace MusicPlayer.Models.Recommendations
{
    public class Image
    {
        [JsonProperty("url")]

        public string Url { get; set; }
        [JsonProperty("height")]

        public int Height { get; set; }
        [JsonProperty("width")]

        public int Width { get; set; }
    }
}
