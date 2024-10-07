using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels
{
    public partial class SpotifyRecViewModel : ViewModelBase
    {
        public ObservableCollection<string> Genres;
        public ObservableCollection<string> Artists;
        public ObservableCollection<string> Albums;


        public override string ToString()
        {
            return "Recommendations by Spotify";
        }
    }
}
