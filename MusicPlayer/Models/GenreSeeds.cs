using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicPlayer.Models
{
    /// <summary>
    /// Class structure storing the genre seeds received from the genre seeds endpoint.
    /// </summary>
    public class GenreSeeds
    {
        [JsonProperty("genres")]
        public List<string> Genres { get; set; }
    }
}
