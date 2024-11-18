using MusicPlayer.Interfaces;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MusicPlayer.ViewModels
{
    public partial class GenresViewModel : UnifiedCategoryViewModel
    {


        public GenresViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput, ITaglLibFactory taglLibFactory)
        {
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;
            this.taglLibFactory = taglLibFactory;

        }
        public override void RefreshContent()
        {
            var GenresSet = Properties.MusicFiles.SelectMany(x => x.Genres).ToHashSet();
            RefreshCategory(GenresSet);

        }
        public override void ShowSongsInCategory(object genre)
        {
            SelectedCategory = (string)genre;


            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.Genres.Contains(SelectedCategory)).ToHashSet();
            UpdateSongCategory(filtered);
        }

        public override async void AddSelectedSongs()
        {

            await ToggleCategoryInputModal("genre");
            ModifySelected();
        }

        public override string ToString()
        {
            return "Genres";
        }

        public override void RemoveSong(object song)
        {
            SongItem item = (SongItem)song;
            if (item.Genres.Remove(SelectedCategory))
            {
                RemoveSingleTag(item);
            }
        }

        protected override void RemoveCurrentSong(SongItem song)
        {
            song.Genres.Remove(SelectedCategory);
        }

        protected override void AddCurrentSong(SongItem song)
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
