using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models.Abstracts
{
    public abstract class GenericDisplayItem
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public Bitmap? ImageMap { get; set; }
    }
}
