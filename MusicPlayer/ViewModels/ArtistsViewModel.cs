using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using MusicPlayer.Models;
using MusicPlayer.Shared;


namespace MusicPlayer.ViewModels
{
    public partial class ArtistsViewModel : GenericCoverDisplay
    {
        public ArtistsViewModel()
        {
            
        }
        public ArtistsViewModel(SharedProperties props)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByCategory = new ObservableCollection<SongListItem>();
            Properties = props;
        }
        public override void RefreshContent()
        {
            var Artists = Properties.MusicFiles.SelectMany(x => x.Artists).ToHashSet();
            RefreshCategory(Artists);

        }
        public override void ShowSongsInCategory(object genre)
        {
            SelectedCategory = (string)genre;
            var filtered = Properties.MusicFiles.Where(x => x.Artists.Contains(SelectedCategory));
            UpdateSongCategory(filtered);
        }

        public override string ToString()
        {
            return "Artists";
        }
    }
}
