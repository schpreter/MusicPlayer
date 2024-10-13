using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicPlayer.Models.Recommendations
{
    public class RecommendationObject : ObservableObject
    {
        [JsonProperty("tracks")]
        public List<Track> Tracks { get; set; }

    }
}
