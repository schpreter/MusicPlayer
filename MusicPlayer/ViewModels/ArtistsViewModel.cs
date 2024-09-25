using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using MusicPlayer.Views;


namespace MusicPlayer.ViewModels
{
    public partial class ArtistsViewModel : UnifiedCategoryViewModel
    {
        public ArtistsViewModel()
        {

        }
        public ArtistsViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByCategory = new ObservableCollection<SongItem>();
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;
        }
        public override void RefreshContent()
        {
            HashSet<string> Artists = Properties.MusicFiles.SelectMany(x => x.Artists).ToHashSet();
            RefreshCategory(Artists);

        }
        public override void ShowSongsInCategory(object genre)
        {
            SelectedCategory = (string)genre;
            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.Artists.Contains(SelectedCategory)).ToHashSet();
            UpdateSongCategory(filtered);
        }


        public override async void AddSelectedSongs()
        {
            await ToggleCategoryInputModal("artist");
            //First we change the category that is stored inside the application
            if (SelectedCategory != null)
            {
                var selectedSongs = Properties.MusicFiles.Where(x => x.IsSelected);
                foreach (var song in selectedSongs)
                {

                    if (!song.Artists.Contains(SelectedCategory))
                    {
                        song.Artists.Add(SelectedCategory);
                    }
                }
                //Then based on the changed values we save the modifications to the file
                ModifyFiles(selectedSongs, "ARTISTS");
            }



        }

        public override string ToString()
        {
            return "Artists";
        }
    }
}
