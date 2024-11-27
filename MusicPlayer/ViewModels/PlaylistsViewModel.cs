using MusicPlayer.Models;
using MusicPlayer.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MusicPlayer.ViewModels
{
    public partial class PlaylistsViewModel : UnifiedCategoryViewModel
    {
        public PlaylistsViewModel() { }
        public PlaylistsViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByCategory = new ObservableCollection<SongItem> { };
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;

        }
        /// <inheritdoc/>

        public override void RefreshContent()
        {
            var playlistNames = Properties.MusicFiles.SelectMany(x => x.PlayLists).Order().ToHashSet();
            RefreshCategory(playlistNames);
        }
        public override string ToString()
        {
            return "Playlists";
        }
        /// <inheritdoc/>

        public override void ShowSongsInCategory(object playlist)
        {
            SelectedCategory = (string)playlist;
            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.PlayLists.Contains(SelectedCategory)).ToHashSet();
            UpdateSongCategory(filtered);
        }
        /// <inheritdoc/>

        public override async void AddSelectedSongs()
        {
            await ToggleCategoryInputModal("playlist");
            ModifySelectedSongs();
        }
        /// <inheritdoc/>

        public override void RemoveSingleSong(object song)
        {
            SongItem item = (SongItem)song;
            if (item.PlayLists.Remove(SelectedCategory))
            {
                ModifyFile(item);
                ShowSongsInCategory(SelectedCategory);
            }
        }
        /// <inheritdoc/>

        protected override void RemoveSong(SongItem song)
        {
            song.PlayLists.Remove(SelectedCategory);
        }
        /// <inheritdoc/>

        protected override void AddSong(SongItem song)
        {
            if (!song.PlayLists.Contains(SelectedCategory))
            {
                song.PlayLists.Add(SelectedCategory);
            }
        }
        /// <inheritdoc/>

        protected override string GetCategory()
        {
            return nameof(PlaylistsViewModel);
        }
    }
}
