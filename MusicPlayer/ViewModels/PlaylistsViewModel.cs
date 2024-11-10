using MusicPlayer.Models;
using MusicPlayer.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MusicPlayer.ViewModels
{
    public partial class PlaylistsViewModel : UnifiedCategoryViewModel
    {
        public PlaylistsViewModel() { }
        public PlaylistsViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByCategory = new ObservableCollection<SongItem> { };
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;

        }

        public override void RefreshContent()
        {
            var playlistNames = Properties.MusicFiles.SelectMany(x => x.PlayLists).ToHashSet();
            RefreshCategory(playlistNames, nameof(PlaylistsViewModel));
        }
        public override string ToString()
        {
            return "Playlists";
        }

        public override void ShowSongsInCategory(object playlist)
        {
            SelectedCategory = (string)playlist;
            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.PlayLists.Contains(SelectedCategory)).ToHashSet();
            UpdateSongCategory(filtered,nameof(PlaylistsViewModel));
        }

        public override async void AddSelectedSongs()
        {
            await ToggleCategoryInputModal("playlist");

            if (SelectedCategory != null)
            {
                var selectedSongs = Properties.MusicFiles.Where(x => x.IsSelected);
                //First we change the category that is stored inside the application
                foreach (var song in selectedSongs)
                {

                    if (!song.PlayLists.Contains(SelectedCategory))
                    {
                        song.PlayLists.Add(SelectedCategory);
                    }
                }
                //Then based on the changed values we save the modifications to the file
                ModifyFiles(selectedSongs, "PLAYLISTS");
            }
        }

        public override void RemoveSelectedSongs()
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveSong(object song)
        {
            SongItem item = (SongItem)song;
            if (item.PlayLists.Remove(SelectedCategory))
            {
                RemoveSingleTag(item, nameof(PlaylistsViewModel));
            }
        }
    }
}
