using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class PlaylistItem
    {
        public string Name {  get; set; }
        public PlaylistItem(string name)
        {
            Name = name;
        }
    }
}
