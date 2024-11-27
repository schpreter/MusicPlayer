using MusicPlayer.Models;
using MusicPlayer.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MusicPlayer.ViewModels
{
    public partial class AlbumsViewModel : UnifiedCategoryViewModel
    {
        public AlbumsViewModel()
        {
            
        }
        public AlbumsViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput)
        {
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;
        }

        public override void RefreshContent()
        {
            //Songs that are not in albums should not even appear as an "album"
            var AlbumSet = Properties.MusicFiles.Select(x => x.Album).Where(x => !string.IsNullOrEmpty(x)).Order().ToHashSet();
            RefreshCategory(AlbumSet);

        }
        public override void ShowSongsInCategory(object album)
        {
            SelectedCategory = (string)album;
            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.Album == SelectedCategory && !string.IsNullOrEmpty(x.Album)).OrderBy(x => x.Title).ToHashSet();
            UpdateSongCategory(filtered);
        }

        public override async void AddSelectedSongs()
        {
            await ToggleCategoryInputModal("album");
            ModifySelectedSongs();
        }

        protected override void AddSong(SongItem song)
        {
            if (!song.Album.Contains(SelectedCategory))
            {
                song.Album = SelectedCategory;
            }
        }

        protected override void RemoveSong(SongItem song)
        {
            if (song.Album == SelectedCategory)
            {
                song.Album = string.Empty;
            }
        }

        public override void RemoveSingleSong(object song)
        {
            {
                SongItem item = (SongItem)song;
                if (item.Album == SelectedCategory && !string.IsNullOrEmpty(SelectedCategory))
                {
                    item.Album = string.Empty;
                    ModifyFile(item);
                    ShowSongsInCategory(SelectedCategory);

                }
            }
        }
        public override string ToString()
        {
            return "Albums";
        }

        protected override string GetCategory()
        {
            return nameof(AlbumsViewModel);
        }
    }
}
