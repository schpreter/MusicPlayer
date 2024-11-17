﻿using MusicPlayer.Models;
using MusicPlayer.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace MusicPlayer.ViewModels
{
    public partial class ArtistsViewModel : UnifiedCategoryViewModel
    {
        public ArtistsViewModel()
        {

        }
        public ArtistsViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput)
        {
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;
        }
        public override void RefreshContent()
        {
            var ArtistsSet = Properties.MusicFiles.SelectMany(x => x.Artists).ToHashSet();
            RefreshCategory(ArtistsSet, nameof(ArtistsViewModel));


        }
        public override void ShowSongsInCategory(object genre)
        {
            SelectedCategory = (string)genre;
            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.Artists.Contains(SelectedCategory)).ToHashSet();
            UpdateSongCategory(filtered, nameof(ArtistsViewModel));
        }


        public override async void AddSelectedSongs()
        {
            await ToggleCategoryInputModal("artist");
            //First we change the category that is stored inside the application
            if (SelectedCategory != null)
            {
                var selectedSongs = Properties.MusicFiles.Where(x => x.IsSelected);
                foreach (var song in selectedSongs)
                {

                    if (!song.Artists.Contains(SelectedCategory))
                    {
                        song.Artists.Add(SelectedCategory);
                    }
                }
                //Then based on the changed values we save the modifications to the file
                ModifyFiles(selectedSongs, nameof(ArtistsViewModel));
            }



        }

        public override string ToString()
        {
            return "Artists";
        }

        public override void RemoveSelectedSongs()
        {
            if (SelectedCategory != null)
            {
                var selectedSongs = Properties.MusicFiles.Where(x => x.IsSelected);
                //First we change the category that is stored inside the application
                foreach (var song in selectedSongs)
                {
                    song.Artists.Remove(SelectedCategory);

                }
                //Then based on the changed values we save the modifications to the file
                ModifyFiles(selectedSongs, nameof(ArtistsViewModel));
            }
        }

        public override void RemoveSong(object song)
        {
            SongItem item = (SongItem)song;
            if (item.Artists.Remove(SelectedCategory))
            {
                RemoveSingleTag(item, nameof(ArtistsViewModel));
            }
        }
    }
}
