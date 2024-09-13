using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using MusicPlayer.Models;
using MusicPlayer.Shared;


namespace MusicPlayer.ViewModels
{
    public partial class ArtistsViewModel : UnifiedCategoryViewModel
    {
        public ArtistsViewModel()
        {

        }
        public ArtistsViewModel(SharedProperties props)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByCategory = new ObservableCollection<SongListItem>();
            Properties = props;
        }
        public override void RefreshContent()
        {
            var Artists = Properties.MusicFiles.SelectMany(x => x.Artists).ToHashSet();
            RefreshCategory(Artists);

        }
        public override void ShowSongsInCategory(object genre)
        {
            SelectedCategory = (string)genre;
            var filtered = Properties.MusicFiles.Where(x => x.Artists.Contains(SelectedCategory));
            UpdateSongCategory(filtered);
        }


        public override async void AddSelectedSongs()
        {
            await ToggleCategoryInputModal();
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
