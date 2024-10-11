using Microsoft.AspNetCore.WebUtilities;
using MusicPlayer.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.API
{
    public static class APICallHandler
    {
        private static string UrlBase = "https://api.spotify.com/v1";
        public static async Task<HttpResponseMessage> GetAvaliableGenreSeeds(HttpClient client)
        {
            return await client.GetAsync(new Uri(UrlBase + "/recommendations/available-genre-seeds"));

        }

        public static async Task<HttpResponseMessage> GetRecommendations(HttpClient client, List<SelectableItem> genreSeeds)
        {
            string recommendationsUrl = UrlBase + "/recommendations";
            string queryGenreSeeds = string.Join(',', genreSeeds.Select(x => x.Display));
            recommendationsUrl = QueryHelpers.AddQueryString(recommendationsUrl, new Dictionary<string, string>()
            {
                {"seed_genres" ,  queryGenreSeeds}
                //Later on other parameters can be added, if there is time to do that
            });
            return await client.GetAsync(new Uri(recommendationsUrl));
        }
    }
}
