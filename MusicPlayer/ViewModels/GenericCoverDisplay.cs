using MusicPlayer.Models.Abstracts;
using System.Collections.ObjectModel;

namespace MusicPlayer.ViewModels
{
    public abstract class GenericCoverDisplay : ViewModelBase
    {
        private ObservableCollection<GenericDisplayItem> itemCollection;
        public ObservableCollection<GenericDisplayItem> ItemCollection
        {
            get { return itemCollection; }
            set { SetProperty(ref itemCollection, value); }
        }
    }
}
