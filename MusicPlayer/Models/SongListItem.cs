using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class SongListItem
    {
        public string Artist { get; set; }
        public string Title { get; set; }

        public SongListItem(string artist, string title) 
        { 
            Artist = artist;
            Title = title;
        }
    }
}
