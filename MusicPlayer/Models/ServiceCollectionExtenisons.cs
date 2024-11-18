using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.Interfaces;
using MusicPlayer.Shared;
using MusicPlayer.ViewModels;
using MusicPlayer.Views;
using System.Net.Http;

namespace MusicPlayer.Models
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<SharedProperties>()
            .AddSingleton<MainWindow>()
            .AddSingleton<ControlWidget>()
            .AddSingleton<HttpClient>()
            .AddSingleton<ITaglLibFactory, TagLibFactory>()
            #region ViewModels
            .AddSingleton<NewCategoryInputViewModel>()
            .AddSingleton<HomeContentViewModel>()
            .AddSingleton<PlaylistsViewModel>()
            .AddSingleton<ArtistsViewModel>()
            .AddSingleton<AlbumsViewModel>()
            .AddSingleton<GenresViewModel>()
            .AddSingleton<SpotifyRecViewModel>()
            .AddSingleton<CurrentSongViewModel>()
            .AddSingleton<MusicNavigationViewModel>()
            .AddSingleton<MainViewModel>();
            #endregion

        }
    }
}
