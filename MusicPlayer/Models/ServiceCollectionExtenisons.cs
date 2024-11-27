using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.Shared;
using MusicPlayer.ViewModels;
using MusicPlayer.Views;
using System.Net.Http;

namespace MusicPlayer.Models
{    /// <summary>
     /// Responsible for registering all the required services for Dependency Injection.
     /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<SharedProperties>()
            .AddSingleton<MainWindow>()
            .AddSingleton<ControlWidget>()
            .AddSingleton<HttpClient>()
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
