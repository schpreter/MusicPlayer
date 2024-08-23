using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MusicPlayer.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class ArtistItem : GenericDisplayItem
    {
        public ArtistItem(string name, string imgpath = null)
        {
            Name = name;
            if(imgpath != null)
            {
                ImagePath = imgpath;
                ImageMap = new Bitmap(AssetLoader.Open(new Uri(imgpath)));
            }

        }
    }
}
