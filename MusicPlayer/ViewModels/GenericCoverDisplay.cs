using MusicPlayer.Models.Abstracts;
using System.Collections.ObjectModel;

namespace MusicPlayer.ViewModels
{
    public abstract class GenericCoverDisplay : ViewModelBase
    {
        public ObservableCollection<UnifiedDisplayItem> ItemCollection { get; set; }
    }
}
