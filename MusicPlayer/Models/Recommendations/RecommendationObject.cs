using Newtonsoft.Json;

namespace MusicPlayer.Models.Recommendations
{
    public class RecommendationObject
    {
        [JsonProperty("tracks")]
        public Tracks Tracks { get; set; }

    }
}
