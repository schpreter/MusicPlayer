using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using MusicPlayer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace MusicPlayer.ViewModels
{
    public partial class PlaylistsViewModel : UnifiedCategoryViewModel
    {
        public PlaylistsViewModel() { }
        public PlaylistsViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            this.Properties = props;
            NewCategoryInputViewModel = newCategoryInput;

        }

        public override void RefreshContent()
        {
            var playlistNames = Properties.MusicFiles.SelectMany(x => x.PlayLists).ToHashSet();
            RefreshCategory(playlistNames);
        }
        public override string ToString()
        {
            return "Playlists";
        }

        public override void ShowSongsInCategory(object playlist)
        {
            SelectedCategory = (string)playlist;
            HashSet<SongListItem> filtered = Properties.MusicFiles.Where(x => x.PlayLists.Contains(SelectedCategory)).ToHashSet();
            UpdateSongCategory(filtered);
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
    }
}
