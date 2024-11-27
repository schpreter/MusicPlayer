using MusicPlayer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels
{
    public partial class CurrentSongViewModel : ViewModelBase
    {
        public CurrentSongViewModel()
        {
            
        }
        public CurrentSongViewModel(SharedProperties props)
        {
            Properties = props;
        }
    }
}
