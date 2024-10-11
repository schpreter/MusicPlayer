using Microsoft.Extensions.DependencyInjection;
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
            #region ViewModels
            .AddTransient<NewCategoryInputViewModel>()
            .AddTransient<HomeContentViewModel>()
            .AddTransient<PlaylistsViewModel>()
            .AddTransient<ArtistsViewModel>()
            .AddTransient<AlbumsViewModel>()
            .AddTransient<GenresViewModel>()
            .AddTransient<SpotifyRecViewModel>()
            .AddTransient<MusicNavigationViewModel>()
            .AddTransient<MainViewModel>();
            #endregion

        }
    }
}
