using MusicPlayer.Models;
using MusicPlayer.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.ViewModels
{
    public partial class GenresViewModel : UnifiedCategoryViewModel
    {
        public GenresViewModel()
        {

        }

        public GenresViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput)
        {
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;

        }
        /// <inheritdoc/>

        public override void RefreshContent()
        {
            var GenresSet = Properties.MusicFiles.SelectMany(x => x.Genres).Order().ToHashSet();
            RefreshCategory(GenresSet);

        }
        /// <inheritdoc/>

        public override void ShowSongsInCategory(object genre)
        {
            SelectedCategory = (string)genre;


            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.Genres.Contains(SelectedCategory)).OrderBy(x => x.Title).ToHashSet();
            UpdateSongCategory(filtered);
        }
        /// <inheritdoc/>

        public override async void AddSelectedSongs()
        {

            await ToggleCategoryInputModal("genre");
            ModifySelectedSongs();
        }
        /// <inheritdoc/>

        public override string ToString()
        {
            return "Genres";
        }
        /// <inheritdoc/>

        public override void RemoveSingleSong(object song)
        {
            SongItem item = (SongItem)song;
            if (item.Genres.Remove(SelectedCategory))
            {
                ModifyFile(item);
                ShowSongsInCategory(SelectedCategory);
            }
        }
        /// <inheritdoc/>

        protected override void RemoveSong(SongItem song)
        {
            song.Genres.Remove(SelectedCategory);
        }
        /// <inheritdoc/>

        protected override void AddSong(SongItem song)
        {
            if (!song.Genres.Contains(SelectedCategory))
            {
                song.Genres.Add(SelectedCategory);
            }
        }
        /// <inheritdoc/>

        protected override string GetCategory()
        {
            return nameof(GenresViewModel);
        }
    }
}
