using MusicPlayer.Models;
using MusicPlayer.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.ViewModels
{
    /// <summary>
    /// Class inheriting from <c>UnifiedCategoryViewModel</c>, for Albums.
    /// </summary>
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
        /// <inheritdoc/>
        public override void RefreshContent()
        {
            //Songs that are not in albums should not even appear as an "album"
            var AlbumSet = Properties.MusicFiles.Select(x => x.Album).Where(x => !string.IsNullOrEmpty(x)).Order().ToHashSet();
            RefreshCategory(AlbumSet);

        }
        /// <inheritdoc/>
        public override void ShowSongsInCategory(object album)
        {
            SelectedCategory = (string)album;
            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.Album == SelectedCategory && !string.IsNullOrEmpty(x.Album)).OrderBy(x => x.Title).ToHashSet();
            UpdateSongCategory(filtered);
        }
        /// <inheritdoc/>
        public override async void AddSelectedSongs()
        {
            await ToggleCategoryInputModal("album");
            ModifySelectedSongs();
        }
        /// <inheritdoc/>
        protected override void AddSong(SongItem song)
        {
            if (!song.Album.Contains(SelectedCategory))
            {
                song.Album = SelectedCategory;
            }
        }
        /// <inheritdoc/>
        protected override void RemoveSong(SongItem song)
        {
            if (song.Album == SelectedCategory)
            {
                song.Album = string.Empty;
            }
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public override string ToString()
        {
            return "Albums";
        }
        /// <inheritdoc/>
        protected override string GetCategory()
        {
            return nameof(AlbumsViewModel);
        }
    }
}
