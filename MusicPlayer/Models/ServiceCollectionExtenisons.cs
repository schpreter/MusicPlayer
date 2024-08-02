using LibVLCSharp.Shared;
using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.Shared;
using MusicPlayer.ViewModels;
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
            collection.AddSingleton<SharedProperties>();
            collection.AddTransient<HomeContentViewModel>();
            collection.AddTransient<PlaylistsViewModel>();
            collection.AddTransient<ArtistsViewModel>();
            collection.AddTransient<AlbumsViewModel>();
            collection.AddTransient<GenresViewModel>();
            collection.AddTransient<MusicNavigationViewModel>();
            collection.AddTransient<MainViewModel>();

        }
    }
}
