using MusicPlayer.Interfaces;
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
        public AlbumsViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput, ITaglLibFactory taglLibFactory)
        {
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;
            this.taglLibFactory = taglLibFactory;
        }

        public override void RefreshContent()
        {
            //Songs that are not in albums should not even appear as an "album"
            var AlbumSet = Properties.MusicFiles.Select(x => x.Album).Where(x => !string.IsNullOrEmpty(x)).ToHashSet();
            RefreshCategory(AlbumSet);

        }
        public override void ShowSongsInCategory(object album)
        {
            SelectedCategory = (string)album;
            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.Album == SelectedCategory && !string.IsNullOrEmpty(x.Album)).ToHashSet();
            UpdateSongCategory(filtered);
        }

        public override async void AddSelectedSongs()
        {
            await ToggleCategoryInputModal("album");
            ModifySelected();
        }

        protected override void AddCurrentSong(SongItem song)
        {
            if (!song.Album.Contains(SelectedCategory))
            {
                song.Album = SelectedCategory;
            }
        }

        protected override void RemoveCurrentSong(SongItem song)
        {
            if (song.Album == SelectedCategory)
            {
                song.Album = string.Empty;
            }
        }

        public override void RemoveSong(object song)
        {
            {
                SongItem item = (SongItem)song;
                if (item.Album == SelectedCategory && !string.IsNullOrEmpty(SelectedCategory))
                {
                    item.Album = string.Empty;
                    RemoveSingleTag(item);
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
