using MusicPlayer.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
