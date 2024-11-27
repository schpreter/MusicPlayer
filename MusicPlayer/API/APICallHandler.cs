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
    /// <summary>
    /// Static class, with a collection of methods relating to making API calls. 
    /// </summary>
    public static class APICallHandler
    {
        private static string UrlBase = "https://api.spotify.com/v1";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client">Client, which will handle the communication with the API</param>
        /// <param name="authorization">Object containing the user's authorization data</param>
        /// <param name="code">The code received after providing access to the user profile</param>
        /// <param name="verifier">The code verifier for the PKCE flow (<see href="https://oauth.net/2/pkce/"/>)</param>
        /// <returns></returns>
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
        /// <summary>
        /// Fetches the genre seeds from the /recommendations/available-genre-seeds endpoint
        /// </summary>
        /// <param name="client">Client, which will handle the communication with the API</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAvaliableGenreSeeds(HttpClient client)
        {
            return await client.GetAsync(new Uri(UrlBase + "/recommendations/available-genre-seeds"));

        }
        /// <summary>
        /// Fetches recommendations from the /recommendations, using the supplied genreSeeds as query parameters
        /// </summary>
        /// <param name="client">Client, which will handle the communication with the API</param>
        /// <param name="genreSeeds">The selected genre seeds, required for getting recommendations</param>
        /// <returns></returns>
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
