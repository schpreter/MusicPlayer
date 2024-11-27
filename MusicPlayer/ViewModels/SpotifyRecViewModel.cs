using CommunityToolkit.Mvvm.ComponentModel;
using DialogHostAvalonia;
using MusicPlayer.API;
using MusicPlayer.Models;
using MusicPlayer.Models.Recommendations;
using MusicPlayer.Shared;
using MusicPlayer.ViewModels.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels
{
    /// <summary>
    /// Classs for which the <c>SpotifyRecView</c> view binds to.
    /// </summary>
    public partial class SpotifyRecViewModel : ViewModelBase
    {
        private const int LIMIT = 5;

        [ObservableProperty]
        public bool isUnderLimit = true;
        public ObservableCollection<SelectableItem> Genres { get; set; }

        [ObservableProperty]
        public RecommendationObject recommendations;

        public HttpClient Client { get; set; }


        public SpotifyRecViewModel()
        {

        }
        public SpotifyRecViewModel(SharedProperties props, HttpClient client)
        {
            Genres = new ObservableCollection<SelectableItem>();
            Recommendations = new RecommendationObject();
            Properties = props;
            Client = client;
        }

        /// <summary>
        /// Calls the appropriate method for getting the genre seeds.
        /// </summary>
        /// <returns>An awaitable <c>Task</c>.</returns>
        public virtual async Task GetAvaliableGenreSeeds()
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
                //TODO: deal with failure, possibly with the button showing up
                Console.WriteLine(ex.Message);
            }


        }
        /// <summary>
        /// Calls the appropriate method for getting recommendations.
        /// </summary>
        /// <returns>An awaitable <c>Task</c>.</returns>
        public virtual async Task GetRecommendations()
        {
            var selectedGenres = Genres.Where(x => x.IsSelected);
            //Validations
            if (selectedGenres.Count() == 0 || selectedGenres.Count() > LIMIT)
            {
                await DialogHost.Show(new GenericNotificationModal("Error", "Please select minimum 1 but maximum 5 genres!"));
            }
            else
            {
                try
                {
                    HttpResponseMessage response = await APICallHandler.GetRecommendations(Client, selectedGenres.ToList());
                    response.EnsureSuccessStatusCode();
                    var content = response.Content.ReadAsStringAsync();
                    Recommendations = JsonConvert.DeserializeObject<RecommendationObject>(content.Result);


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
