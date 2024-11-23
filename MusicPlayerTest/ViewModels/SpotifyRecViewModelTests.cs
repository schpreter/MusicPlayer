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
            SpotifyRecViewModel vm = new SpotifyRecViewModel(_properties.Object,new HttpClient(handlerMock.Object));

            await vm.GetAvaliableGenreSeeds();

            Assert.Collection(vm.Genres, item => Assert.Equal("alternative",item.Display)
                                       , item => Assert.Equal("samba",item.Display));
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
        public void GetRecommendationsTest_Success()
        {
            Xunit.Assert.Fail("This test needs an implementation");
        }

    }
}