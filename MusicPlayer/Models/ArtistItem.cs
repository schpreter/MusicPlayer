using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class ArtistItem
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public Bitmap? ImageMap { get; set; }

        public ArtistItem(string name, string imgpath)
        {
            Name = name;
            ImagePath = imgpath;
            ImageMap = new Bitmap(AssetLoader.Open(new Uri(imgpath)));
        }
    }
}
