using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class GenreSeeds
    {
        [JsonProperty("genres")]
        public List<string> Genres { get; set; }
    }
}
