using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using DialogHostAvalonia;
using LibVLCSharp.Shared;
using Microsoft.VisualBasic;
using MusicPlayer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels
{
    public abstract partial class UnifiedCategoryViewModel : ViewModelBase
    {
        protected NewCategoryInputViewModel NewCategoryInputViewModel { get; set; }
        public ObservableCollection<SongListItem> SongsByCategory { get; set; }
        public ObservableCollection<UnifiedDisplayItem> ItemCollection { get; set; }

        [ObservableProperty]
        public string selectedCategory;

        [ObservableProperty]
        public bool showSongs = false;

        [ObservableProperty]
        public bool showSongSelectionList = false;

        [ObservableProperty]
        public bool showCategoryHome = true;

        [ObservableProperty]
        public bool newCategoryDialogOpen = false;

        public abstract void ShowSongsInCategory(object category);
        public abstract void AddSelectedSongs();

        public void ShowHome()
        {
            ShowSongSelectionList = false;
            ShowSongs = false;
            ShowCategoryHome = true;
        }
        public void ShowSelection()
        {
            ShowSongSelectionList = true;
            ShowSongs = false;
            ShowCategoryHome = false;

        }
        public void AddNewCategory()
        {
            SelectedCategory = null;
            ShowSelection();
        }

        protected void UpdateSongCategory(HashSet<SongListItem> filtered)
        {
            SongsByCategory.Clear();
            foreach (var item in filtered)
            {
                SongsByCategory.Add((SongListItem)item);
            }
            ShowSongs = true;
            ShowCategoryHome = false;
        }

        protected void RefreshCategory(HashSet<string> set)
        {
            ItemCollection.Clear();
            foreach (var item in set)
            {
                ItemCollection.Add(new UnifiedDisplayItem(item));
            }
            ShowHome();
        }
        protected async Task ToggleCategoryInputModal(string categoryType)
        {
            //First, if the selected category is null, we must prompt the user to select a category
            if (SelectedCategory == null)
            {
                NewCategoryInputViewModel.Title = $"New {categoryType}:";
                NewCategoryInputViewModel.Description = $"Enter your new {categoryType}.";
                SelectedCategory = (string)await DialogHost.Show(NewCategoryInputViewModel);
            }

        }

        protected void ModifyFiles(IEnumerable songs, string category)
        {
            foreach (SongListItem song in songs)
            {
                TagLib.File tagLibFile = TagLib.File.Create(song.FilePath);
                switch (category)
                {
                    case "GENRES":
                        {
                            tagLibFile.Tag.Genres = song.Genres.ToArray();
                            song.Genres = tagLibFile.Tag.Genres.ToList();
                            break;
                        }
                    case "ARTISTS":
                        {
                            tagLibFile.Tag.Album = song.Album;
                            break;
                        }
                    case "ALBUM":
                        {
                            tagLibFile.Tag.Performers = song.Artists.ToArray();
                            song.Artists_conc = tagLibFile.Tag.JoinedPerformers;
                            break;
                        }
                        //This is where the file format matters, just like during parsing
                    case "PLAYLISTS":
                        {
                            break;
                        }
                    default:
                        break;
                }
                tagLibFile.Save();
            }
            RefreshContent();
            ShowHome();
        }


    }
}
