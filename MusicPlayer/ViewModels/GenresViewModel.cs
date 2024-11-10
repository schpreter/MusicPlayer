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
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByCategory = new ObservableCollection<SongItem> { };
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;
        }
        public override void RefreshContent()
        {
            var GenresSet = Properties.MusicFiles.SelectMany(x => x.Genres).ToHashSet();
            RefreshCategory(GenresSet, nameof(GenresViewModel));

        }
        public override void ShowSongsInCategory(object genre)
        {
            SelectedCategory = (string)genre;


            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.Genres.Contains(SelectedCategory)).ToHashSet();
            UpdateSongCategory(filtered,nameof(GenresViewModel));
        }

        public override async void AddSelectedSongs()
        {

            await ToggleCategoryInputModal("genre");

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
                ModifyFiles(selectedSongs, nameof(GenresViewModel));
            }
        }

        public override string ToString()
        {
            return "Genres";
        }

        public override void RemoveSelectedSongs()
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveSong(object song)
        {
            SongItem item = (SongItem)song;
            if (item.Genres.Remove(SelectedCategory))
            {
                RemoveSingleTag(item, nameof(GenresViewModel));
            }
        }
    }
}
