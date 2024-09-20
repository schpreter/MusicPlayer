using MusicPlayer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MusicPlayer.Shared;

namespace MusicPlayer.ViewModels
{
    public partial class AlbumsViewModel : UnifiedCategoryViewModel
    {
        public AlbumsViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByCategory = new ObservableCollection<SongListItem>();
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;
        }

        public override void RefreshContent()
        {
            HashSet<string> Albums = Properties.MusicFiles.Select(x => x.Album).ToHashSet();
            RefreshCategory(Albums);

        }
        public override void ShowSongsInCategory(object album)
        {
            SelectedCategory = (string)album;
            HashSet<SongListItem> filtered = Properties.MusicFiles.Where(x => x.Album == SelectedCategory).ToHashSet();
            UpdateSongCategory(filtered);
        }

        public override async void AddSelectedSongs()
        {
            await ToggleCategoryInputModal("album");

            //First we change the category that is stored inside the application
            if (SelectedCategory != null)
            {
                var selectedSongs = Properties.MusicFiles.Where(x => x.IsSelected);
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
        }

        public override string ToString()
        {
            return "Albums - " + SelectedCategory;
        }
    }
}
