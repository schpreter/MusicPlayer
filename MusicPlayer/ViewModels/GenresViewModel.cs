using MusicPlayer.Models;
using System.Collections.ObjectModel;
using System.Linq;
using MusicPlayer.Shared;
using TagLib.Ape;
using System;
using DialogHostAvalonia;

namespace MusicPlayer.ViewModels
{
    public partial class GenresViewModel : UnifiedCategoryViewModel
    {


        public GenresViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByCategory = new ObservableCollection<SongListItem> { };
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;
        }
        public override void RefreshContent()
        {
            var Genres = Properties.MusicFiles.SelectMany(x => x.Genres).ToHashSet();
            RefreshCategory(Genres);

        }
        public override void ShowSongsInCategory(object genre)
        {
            SelectedCategory = (string)genre;
            var filtered = Properties.MusicFiles.Where(x => x.Genres.Contains(SelectedCategory));
            UpdateSongCategory(filtered);
        }

        public override async void AddSelectedSongs()
        {


            await ToggleCategoryInputModal();

            if (SelectedCategory != null)
            {
                var selectedSongs = Properties.MusicFiles.Where(x => x.IsSelected);
                //First we change the category that is stored inside the application
                foreach (var song in selectedSongs)
                {

                    if (!song.Genres.Contains(SelectedCategory))
                    {
                        song.Genres.Add(SelectedCategory);
                    }
                }
                //Then based on the changed values we save the modifications to the file
                ModifyFiles(selectedSongs, "GENRES");
            }
        }

        public override string ToString()
        {
            return "Genres";
        }

    }
}
