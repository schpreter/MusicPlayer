using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicPlayer.Models.Recommendations
{    /// <summary>
     /// Class structure which Newtonsoft can deserialize the recommendations JSON response into.
     /// </summary>
    public class RecommendationObject : ObservableObject
    {
        [JsonProperty("tracks")]
        public List<Track> Tracks { get; set; }

    }
}
