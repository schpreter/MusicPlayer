using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Models;
using MusicPlayer.Shared;
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

        public PlaylistsViewModel(SharedProperties props)
        {
            this.Properties = props;
            //var idk = this.Properties.MusicFiles.GroupBy(x => x.SongMetaData.Artists);

        }

        /*
         * Playlist generation needs tobe seperate from the constructor, as during DI the required fields are unknown/null
         */
        public override void RefreshContent()
        {
            var idk = Properties.MusicFiles.SelectMany(x => x.SongMetaData.Artists).ToHashSet();
        }
    }
}
