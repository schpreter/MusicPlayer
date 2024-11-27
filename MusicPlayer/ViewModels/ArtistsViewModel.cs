using MusicPlayer.Models;
using MusicPlayer.Shared;
using System.Collections.Generic;
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
        /// <inheritdoc/>
        public override void RefreshContent()
        {
            var ArtistsSet = Properties.MusicFiles.SelectMany(x => x.Artists).Order().ToHashSet();
            RefreshCategory(ArtistsSet);


        }
        /// <inheritdoc/>
        public override void ShowSongsInCategory(object genre)
        {
            SelectedCategory = (string)genre;
            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.Artists.Contains(SelectedCategory)).OrderBy(x => x.Title).ToHashSet();
            UpdateSongCategory(filtered);
        }

        /// <inheritdoc/>
        public override async void AddSelectedSongs()
        {
            await ToggleCategoryInputModal("artist");
            ModifySelectedSongs();
        }

        public override string ToString()
        {
            return "Artists";
        }
        /// <inheritdoc/>

        public override void RemoveSingleSong(object song)
        {
            SongItem item = (SongItem)song;
            if (item.Artists.Remove(SelectedCategory))
            {
                ModifyFile(item);
                ShowSongsInCategory(SelectedCategory);

            }

        }
        /// <inheritdoc/>

        protected override void RemoveSong(SongItem song)
        {
            song.Artists.Remove(SelectedCategory);
        }
        /// <inheritdoc/>

        protected override void AddSong(SongItem song)
        {
            if (!song.Artists.Contains(SelectedCategory))
            {
                song.Artists.Add(SelectedCategory);
            }
        }
        /// <inheritdoc/>

        protected override string GetCategory()
        {
            return nameof(ArtistsViewModel);
        }
    }
}
