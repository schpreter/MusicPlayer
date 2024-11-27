using Xunit;
using MusicPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using Moq;
using MusicPlayer.Shared;
using Moq.Protected;
using System.Net;
using TagLib.Ape;
using MusicPlayer.Models.Recommendations;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using MusicPlayer.Models;

namespace MusicPlayer.ViewModels.Tests
{
    public class SpotifyRecViewModelTests
    {
        private readonly Mock<SharedProperties> _properties = new Mock<SharedProperties>();
        private readonly Mock<HttpClient> _client = new Mock<HttpClient>();


        [Fact()]
        public void SpotifyRecViewModelTest()
        {
            SpotifyRecViewModel vm = new SpotifyRecViewModel(_properties.Object, _client.Object);

            Assert.NotNull(vm);
            Assert.Empty(vm.Genres);
            Assert.NotNull(vm.Recommendations);
            Assert.Equal(vm.Properties, _properties.Object);
            Assert.Equal(vm.Client, _client.Object);
        }

        [Fact()]
        public async void GetAvaliableGenreSeedsTest_Success()
        {
            Mock<SpotifyRecViewModel> vmMock = new Mock<SpotifyRecViewModel>(_properties.Object, _client.Object);
            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{  \"genres\": [\"alternative\", \"samba\"]}")
                });
            SpotifyRecViewModel vm = new SpotifyRecViewModel(_properties.Object, new HttpClient(handlerMock.Object));

            await vm.GetAvaliableGenreSeeds();

            Assert.Collection(vm.Genres, item => Assert.Equal("alternative", item.Display)
                                       , item => Assert.Equal("samba", item.Display));
        }

        [Fact()]
        public async void GetAvaliableGenreSeedsTest_Error()
        {
            Mock<SpotifyRecViewModel> vmMock = new Mock<SpotifyRecViewModel>(_properties.Object, _client.Object);
            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent("{\"error\":{\"status\": 400,\"message\":\"string\"}}")
                });
            SpotifyRecViewModel vm = new SpotifyRecViewModel(_properties.Object, new HttpClient(handlerMock.Object));

            // Act
            await vm.GetAvaliableGenreSeeds();

            // Assert
            Assert.Empty(vm.Genres);
        }

        [Fact()]
        public async void GetRecommendationsTest_Success()
        {
            Mock<SpotifyRecViewModel> vmMock = new Mock<SpotifyRecViewModel>(_properties.Object, _client.Object);
            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>();

            RecommendationObject result = new RecommendationObject();

            result.Tracks = new List<Track>();

            //Add 20 random songs as to simulate the Spotify default response, then serialize as json
            for (int i = 0; i < 20; i++) 
            {
                result.Tracks.Add(new Track()
                {
                    Name = $"Name_{i}",
                    Album = new Album()
                    {
                        Name = $"Album_{i}"
                    },
                    Artists = new List<Artist>()
                    {
                        new Artist() {
                            Name = $"Aritst_{i}"}
                    },
                    DurationMs = 2000
                });
            }
            string jsonResponse = JsonConvert.SerializeObject(result, Formatting.Indented);

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });
            SpotifyRecViewModel vm = new SpotifyRecViewModel(_properties.Object, new HttpClient(handlerMock.Object));

            vm.Genres = new ObservableCollection<SelectableItem>() {  new SelectableItem() { Display = "rock", IsSelected= true}, new SelectableItem() { Display = "metal", IsSelected = false}, };

            // Act
            await vm.GetRecommendations();

            // Assert
            Assert.Equivalent(vm.Recommendations, JsonConvert.DeserializeObject<RecommendationObject>(jsonResponse));
        }

        [Fact()]
        public async void GetRecommendationsTest_Error()
        {
            Mock<SpotifyRecViewModel> vmMock = new Mock<SpotifyRecViewModel>(_properties.Object, _client.Object);
            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent("N/A") //TDue to ensure succes code we dont care about the data anyway
                });
            SpotifyRecViewModel vm = new SpotifyRecViewModel(_properties.Object, new HttpClient(handlerMock.Object));

            vm.Genres = new ObservableCollection<SelectableItem>() { new SelectableItem() { Display = "rock", IsSelected = true }, new SelectableItem() { Display = "metal", IsSelected = false }, };

            // Act
            await vm.GetRecommendations();

            // Assert
            Assert.Equivalent(vm.Recommendations, new RecommendationObject());
        }

    }
}