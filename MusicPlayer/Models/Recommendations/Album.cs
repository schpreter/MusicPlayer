using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.Models.Recommendations
{
    public class Album
    {
        [JsonProperty("album_type")]

        public string AlbumType { get; set; }
        [JsonProperty("total_tracks")]

        public int TotalTracks { get; set; }
        [JsonProperty("avaliable_markets")]

        public List<string> AvaliableMarkets { get; set; }
        [JsonProperty("external_urls")]


        public ExternalUrls ExternalUrls { get; set; }
        [JsonProperty("href")]


        public string Href { get; set; }
        [JsonProperty("id")]

        public string Id { get; set; }
        [JsonProperty("images")]


        public List<Image> Images { get; set; }
        [JsonProperty("name")]


        public string Name { get; set; }
        [JsonProperty("release_date")]

        public string ReleaseDate { get; set; }
        [JsonProperty("restrictions")]


        public Restrictions Restrictions { get; set; }
        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; }

        public Image DefaultImage
        {
            get
            {
                try
                {
                    return Images.First();

                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
