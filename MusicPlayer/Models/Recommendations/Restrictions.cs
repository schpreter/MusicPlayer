using Newtonsoft.Json;

namespace MusicPlayer.Models.Recommendations
{
    public class Restrictions
    {
        [JsonProperty("reason")]

        public string Reason { get; set; }
    }
}
