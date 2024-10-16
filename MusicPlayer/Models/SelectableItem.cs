using CommunityToolkit.Mvvm.ComponentModel;

namespace MusicPlayer.Models
{
    public partial class SelectableItem : ObservableObject
    {
        [ObservableProperty]
        public bool isSelected;

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
