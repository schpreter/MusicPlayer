using MusicPlayer.Models;
using MusicPlayer.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MusicPlayer.ViewModels
{
    public partial class GenresViewModel : UnifiedCategoryViewModel
    {


        public GenresViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput)
        {
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;

        }
        public override void RefreshContent()
        {
            var GenresSet = Properties.MusicFiles.SelectMany(x => x.Genres).Order().ToHashSet();
            RefreshCategory(GenresSet);

        }
        public override void ShowSongsInCategory(object genre)
        {
            SelectedCategory = (string)genre;


            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.Genres.Contains(SelectedCategory)).OrderBy(x => x.Title).ToHashSet();
            UpdateSongCategory(filtered);
        }

        public override async void AddSelectedSongs()
        {

            await ToggleCategoryInputModal("genre");
            ModifySelectedSongs();
        }

        public override string ToString()
        {
            return "Genres";
        }

        public override void RemoveSingleSong(object song)
        {
            SongItem item = (SongItem)song;
            if (item.Genres.Remove(SelectedCategory))
            {
                ModifyFile(item);
                ShowSongsInCategory(SelectedCategory);
            }
        }

        protected override void RemoveSong(SongItem song)
        {
            song.Genres.Remove(SelectedCategory);
        }

        protected override void AddSong(SongItem song)
        {
            if (!song.Genres.Contains(SelectedCategory))
            {
                song.Genres.Add(SelectedCategory);
            }
        }

        protected override string GetCategory()
        {
            return nameof(GenresViewModel);
        }
    }
}
