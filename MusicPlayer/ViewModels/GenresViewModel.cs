using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Shared;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MusicPlayer.ViewModels
{
    public partial class GenresViewModel : GenericCoverDisplay
    {
        public ObservableCollection<SongListItem> SongsByGenre { get; set; }

        [ObservableProperty]
        public bool showSongs;

        [ObservableProperty]
        public bool showGenres;

        [ObservableProperty]
        public string selectedGenre;
        public GenresViewModel(SharedProperties props)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByGenre = new ObservableCollection<SongListItem> { };
            Properties = props;
            ShowSongs = false;
            ShowGenres = true;

        }
        public override void RefreshContent()
        {
            var Genres = Properties.MusicFiles.SelectMany(x => x.Genres).ToHashSet();
            foreach (var item in Genres)
            {
                if (ItemCollection.All(x => x.Name != item))
                    ItemCollection.Add(new UnifiedDisplayItem(item));
            }
        }
        public override void ShowSongsInCategory(object genre)
        {
            SelectedGenre = (string)genre;
            SongsByGenre.Clear();
            var filtered = Properties.MusicFiles.Where(x => x.Genres.Contains(SelectedGenre));
            foreach (var item in filtered)
            {
                SongsByGenre.Add(item);
            }
            ShowSongs = true;
            ShowGenres = false;
        }
    }
}
