using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class SongListItem
    {

        public SongMetaData SongMetaData { get; set; }
        public SongListItem(SongMetaData metaData)
        {
            SongMetaData = metaData;
        }
    }
}
