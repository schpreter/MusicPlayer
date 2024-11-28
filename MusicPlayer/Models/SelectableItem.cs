using CommunityToolkit.Mvvm.ComponentModel;

namespace MusicPlayer.Models
{
    /// <summary>
    /// General class used for items which the user can select with a checkbox.
    /// </summary>
    public partial class SelectableItem : ObservableObject
    {
        [ObservableProperty]
        public bool isSelected;

        public string Display { get; set; } = string.Empty;
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
