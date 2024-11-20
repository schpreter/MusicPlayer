using Microsoft.AspNetCore.WebUtilities;
using MusicPlayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MusicPlayer.API
{
    public static class APICallHandler
    {
        private static string UrlBase = "https://api.spotify.com/v1";

        public static async Task<AuthorizationTokenData> GetAccessTokenAsync(HttpClient client, AuthorizationObject authorization, string code, string verifier)
        {
            string url = "https://accounts.spotify.com/api/token";


            using FormUrlEncodedContent content = new FormUrlEncodedContent(
                new List<KeyValuePair<string, string>>()
                {
                new KeyValuePair<string, string> ( "client_id", authorization.ClientID ),
                new KeyValuePair<string, string> ( "grant_type", "authorization_code" ),
                new KeyValuePair<string, string> ( "code", code ),
                new KeyValuePair<string, string> ( "redirect_uri" , authorization.RedirectUri ),
                new KeyValuePair<string, string> ( "code_verifier" , verifier ),
                }
                );

            //Setting the header's content type   
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            //Post the content to the API    
            HttpResponseMessage response = await client.PostAsync(url, content);
            try
            {
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<AuthorizationTokenData>(response.Content.ReadAsStringAsync().Result);
            }
            catch 
            { 
                return null;
            }

        }

        public static async Task<string> FetchProfile(string token)
        {
            //TODO
            return null;

        }

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
