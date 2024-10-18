using Avalonia.Media.Imaging;

namespace MusicPlayer.Models
{
    public class UnifiedDisplayItem
    {
        public string Name { get; set; }
        public Bitmap ImageMap { get; set; }
        public UnifiedDisplayItem(string name, Bitmap image = null)
        {
            Name = name;
            if (image != null)
            {
                ImageMap = image;
            }

        }
    }
}
