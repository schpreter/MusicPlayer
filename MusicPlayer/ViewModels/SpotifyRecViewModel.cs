using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels
{
    public partial class SpotifyRecViewModel : ViewModelBase
    {
        private const int LIMIT = 5;

        [ObservableProperty]
        public bool isUnderLimit = true;
        public ObservableCollection<string> GenresDrpOptions { get; set; }
        public ObservableCollection<string> Genres { get; set; }

        [ObservableProperty]
        public string genreInput;
        public ObservableCollection<string> ArtistsDrpOptions { get; set; }
        public ObservableCollection<string> AlbumsDrpOptions { get; set; }

        private readonly HttpClient Client;



        public SpotifyRecViewModel(SharedProperties props, HttpClient client)
        {
            GenresDrpOptions = new ObservableCollection<string>();
            Genres = new ObservableCollection<string>();
            Properties = props;
            Client = client;
        }

        public override void RefreshContent()
        {
            var GenreNames = Properties.MusicFiles.SelectMany(x => x.Genres).Distinct();
            foreach (string item in GenreNames)
            {
                if (!GenresDrpOptions.Contains(item))
                    GenresDrpOptions.Add(item);
            }

        }

        public void AddToCollection(string collection)
        {
            switch (collection)
            {
                case "Genres":
                    {
                        if (!Genres.Contains(GenreInput))
                            Genres.Add(GenreInput);
                        GenreInput = String.Empty;
                        break;
                    }
            }
            IsUnderLimit = Genres.Count() < LIMIT;

        }

        public void RemoveItem(object item)
        {
            //switch (item)
            //{
            //    case "Genre":
            //        {
            Genres.Remove((string)item);
            //            break;
            //        }
            //}
            IsUnderLimit = Genres.Count() < LIMIT;

        }

        public void GetRecommendations()
        {

        }

        public override string ToString()
        {
            return "Recommendations by Spotify";
        }
    }
}
