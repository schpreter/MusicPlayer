using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels
{
    public partial class PlaylistsViewModel : ViewModelBase
    {
        private ObservableCollection<PlaylistItem> playlists;
        public ObservableCollection<PlaylistItem> Playlists
        {
            get { return playlists; }
            set { SetProperty(ref playlists, value); }
        }

        public PlaylistsViewModel()
        {
            Playlists = new ObservableCollection<PlaylistItem>
            {
                new PlaylistItem("List1"),
                new PlaylistItem("List2"),
                new PlaylistItem("List3"),
            };
        }
    }
}
