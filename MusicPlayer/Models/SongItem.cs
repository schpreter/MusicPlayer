using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagLib;

namespace MusicPlayer.Models
{
    public class SongItem : SelectableItem
    {

        public string Album { get; set; }
        public string Title { get; set; }
        public List<string> Artists { get; set; } = new List<string>();

        public List<string> Genres { get; set; } = new List<string>();
        public int Year { get; set; }
        public TimeSpan Duration { get; set; }
        public string FilePath { get; set; }
        public List<string> PlayLists { get; set; }
        //TODO: Figure out what to do with these pics
        //For now I will only use the first pics data, read it into a stream and feed that stream to a Bitmap
        public List<ByteVector> Images { get; set; }

        public double Duration_s
        {
            get
            {
                return Duration.TotalMilliseconds;
            }
        }

        public string Duration_display
        {
            get
            {
                return Duration.ToString(@"mm\:ss");
            }
        }
        public string Artists_conc { 
            get
            {
                return string.Join("; ", Artists);
            } 
        }


        public override string ToString()
        {
            return $"{Artists_conc} {Title} {Duration_display}";
        }

        public string Genres_conc
        {
            get
            {
                return string.Join("; ", Genres);
            }
        }

        public Bitmap FirstImage
        {
            get
            {
                if(Images != null)
                {
                    var pic = Images.FirstOrDefault();
                    if (pic != null)
                    {
                        using MemoryStream ms = new MemoryStream(pic.ToArray());
                        return new Bitmap(ms);
                    }
                }
                return null;


            }
        }
    }
}
