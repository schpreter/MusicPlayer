using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using MusicPlayer.Models;
using MusicPlayer.Models.Abstracts;
using MusicPlayer.Shared;


namespace MusicPlayer.ViewModels
{
    public partial class ArtistsViewModel : GenericCoverDisplay
    {
        public ArtistsViewModel(SharedProperties props)
        {
            ItemCollection = new ObservableCollection<GenericDisplayItem>();
            Properties = props;
        }
        public override void RefreshContent()
        {
            var Artists = Properties.MusicFiles.SelectMany(x => x.SongMetaData.Artists).ToHashSet();
            foreach (var item in Artists)
            {
                if (ItemCollection.All(x => x.Name != item))
                    ItemCollection.Add(new ArtistItem(item));
            }
        }
    }
}
