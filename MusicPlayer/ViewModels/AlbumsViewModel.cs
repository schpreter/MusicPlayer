﻿using MusicPlayer.Models;
using MusicPlayer.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MusicPlayer.ViewModels
{
    public partial class AlbumsViewModel : UnifiedCategoryViewModel
    {
        public AlbumsViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByCategory = new ObservableCollection<SongItem>();
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;
        }

        public override void RefreshContent()
        {
            var AlbumSet = Properties.MusicFiles.Select(x => x.Album).ToHashSet();
            RefreshCategory(AlbumSet, nameof(AlbumsViewModel));

        }
        public override void ShowSongsInCategory(object album)
        {
            SelectedCategory = (string)album;
            HashSet<SongItem> filtered = SelectedCategory == string.Empty ? Properties.MusicFiles.Where(x => x.Album == null).ToHashSet() : Properties.MusicFiles.Where(x => x.Album == SelectedCategory).ToHashSet();
            UpdateSongCategory(filtered,nameof(AlbumsViewModel));
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
                ModifyFiles(selectedSongs, nameof(AlbumsViewModel));
            }
        }

        public override string ToString()
        {
            return "Albums";
        }


    }
}
