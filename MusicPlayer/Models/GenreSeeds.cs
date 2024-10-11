using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicPlayer.Models
{
    public class GenreSeeds
    {
        [JsonProperty("genres")]
        public List<string> Genres { get; set; }
    }
}
