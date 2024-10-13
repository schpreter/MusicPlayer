namespace MusicPlayer.Models
{
    public class SelectableItem
    {
        public bool IsSelected { get; set; }

        public string Display { get; set; }
        public SelectableItem()
        {

        }
        public SelectableItem(string display)
        {
            Display = display;
        }

        public override string ToString()
        {
            return Display;
        }
    }
}
