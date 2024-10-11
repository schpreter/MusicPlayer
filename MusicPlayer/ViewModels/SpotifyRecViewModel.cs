using CommunityToolkit.Mvvm.ComponentModel;
using DialogHostAvalonia;
using MusicPlayer.API;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using MusicPlayer.ViewModels.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;

namespace MusicPlayer.ViewModels
{
    public partial class SpotifyRecViewModel : ViewModelBase
    {
        //TODO: Later on this limit is max 5 seed in total, not each
        private const int LIMIT = 5;

        [ObservableProperty]
        public bool isUnderLimit = true;
        public ObservableCollection<SelectableItem> Genres { get; set; }

        //[ObservableProperty]
        //public string genreInput;
        //public ObservableCollection<string> ArtistsDrpOptions { get; set; }
        //public ObservableCollection<string> AlbumsDrpOptions { get; set; }

        private readonly HttpClient Client;



        public SpotifyRecViewModel(SharedProperties props, HttpClient client)
        {
            Genres = new ObservableCollection<SelectableItem>();
            Properties = props;
            Client = client;
        }

        public async void GetAvaliableGenreSeeds()
        {
            try
            {
                HttpResponseMessage response = await APICallHandler.GetAvaliableGenreSeeds(Client);
                response.EnsureSuccessStatusCode();

                var content = response.Content.ReadAsStringAsync();
                GenreSeeds result = JsonConvert.DeserializeObject<GenreSeeds>(content.Result);

                Genres.Clear();
                foreach (string seed in result.Genres)
                {
                    this.Genres.Add(new SelectableItem(seed));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        public async void GetRecommendations()
        {
            var selectedGenres = Genres.Where(x => x.IsSelected);
            //Validations
            if (selectedGenres.Count() == 0 || selectedGenres.Count() > LIMIT)
            {
                await DialogHost.Show(new GenericNotificationModal() { Title = "Error", Message = "Please select minimum 1 but maximum 5 genres!" });
            }
            else
            {
                try
                {
                    HttpResponseMessage response = await APICallHandler.GetRecommendations(Client, selectedGenres.ToList());
                    var content = response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        public override string ToString()
        {
            return "Recommendations by Spotify";
        }
    }
}
