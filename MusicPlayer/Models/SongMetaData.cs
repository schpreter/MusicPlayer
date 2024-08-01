using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class SongMetaData
    {
        public string? Album { get; set; }
        public string Title { get; set; }
        public List<string>? Artists { get; set; }
        public List<string>? Genres { get; set; }
        public int? Year { get; set; }
        public double Duration { get; set; }
    }
}
