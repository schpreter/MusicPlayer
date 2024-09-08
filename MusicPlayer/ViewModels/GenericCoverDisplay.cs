using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagLib;
using TagLib.Ape;

namespace MusicPlayer.ViewModels
{
    public abstract partial class GenericCoverDisplay : ViewModelBase
    {

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
        public virtual void ShowSongsInCategory(object category) { }
        public virtual void ShowSongSelection() { }
        public virtual void AddSelectedSongs() { }
        public void BackToCategoryHome()
        {
            ShowSongSelectionList = false;
            ShowSongs = false;
            ShowCategoryHome = true;
        }

        protected void UpdateSongCategory(IEnumerable filtered)
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
                if (ItemCollection.All(x => x.Name != item))
                    ItemCollection.Add(new UnifiedDisplayItem(item));
            }
            ShowSongSelectionList = false;
            ShowSongs = false;
            ShowCategoryHome = true;
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
                    default:
                        break;
                }
                tagLibFile.Save();
                Notification notif = new Notification("File Save","Songs added successfully");
            }
        }

        public void ShowSelection()
        {
            ShowSongSelectionList = true;
            ShowSongs = false;
            ShowCategoryHome = false;

        }
    }
}
