using MusicPlayer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MusicPlayer.Shared;

namespace MusicPlayer.ViewModels
{
    public partial class AlbumsViewModel : UnifiedCategoryViewModel
    {
        public AlbumsViewModel(SharedProperties props)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByCategory = new ObservableCollection<SongListItem>();
            Properties = props;
        }

        public override void RefreshContent()
        {
            HashSet<string> Albums = Properties.MusicFiles.Select(x => x.Album).ToHashSet();
            RefreshCategory(Albums);

        }
        public override void ShowSongsInCategory(object album)
        {
            SelectedCategory = (string)album;
            var filtered = Properties.MusicFiles.Where(x => x.Album == SelectedCategory);
            UpdateSongCategory(filtered);
        }

        public override void AddSelectedSongs()
        {
            var selectedSongs = Properties.MusicFiles.Where(x => x.IsSelected);
            //First we change the category that is stored inside the application
            foreach (var song in selectedSongs)
            {

                if (!song.Album.Contains(SelectedCategory))
                {
                    song.Album = SelectedCategory;
                }
            }
            //Then based on the changed values we save the modifications to the file
            ModifyFiles(selectedSongs, "ALBUM");

        }

        public override string ToString()
        {
            return "Albums - " + SelectedCategory;
        }
    }
}
