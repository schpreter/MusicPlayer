using MusicPlayer.Models;
using System.Collections.ObjectModel;

namespace MusicPlayer.ViewModels
{
    public abstract class GenericCoverDisplay : ViewModelBase
    {
        public ObservableCollection<UnifiedDisplayItem> ItemCollection { get; set; }
        public virtual void ShowSongsInCategory(object category) { }
    }
}
