using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagLib;

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
        public void BackToCategoryHome()
        {
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
        }

        public void ShowSelection()
        {
            ShowSongSelectionList = true;
            ShowSongs = false;
            ShowCategoryHome = false;

        }


    }
}
