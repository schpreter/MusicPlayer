using Avalonia.Platform.Storage;
using LibVLCSharp.Shared;
using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.Data;
using MusicPlayer.Shared;
using MusicPlayer.ViewModels;
using MusicPlayer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<SharedProperties>()
            .AddSingleton<MainWindow>()
            .AddSingleton<ControlWidget>()
            #region ViewModels
            .AddSingleton<AuthorizationTokenData>()
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
