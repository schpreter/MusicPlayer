using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class SongListItem
    {
        //TODO: delete later, its good for view placehoder now
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }

        public SongMetaData SongMetaData { get; set; }

        public SongListItem(string artist, string title, string duration)
        {
            Artist = artist;
            Title = title;
            Duration = duration;
        }
        public SongListItem(SongMetaData metaData)
        {
            SongMetaData = metaData;
        }
    }
}
