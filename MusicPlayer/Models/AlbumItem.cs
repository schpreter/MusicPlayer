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
    public class AlbumItem : GenericDisplayItem
    {
        public AlbumItem(string name, string imgpath)
        {
            Name = name;
            ImagePath = imgpath;
            ImageMap = new Bitmap(AssetLoader.Open(new Uri(imgpath)));
        }
    }
}
