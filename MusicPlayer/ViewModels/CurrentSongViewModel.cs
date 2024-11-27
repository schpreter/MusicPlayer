using MusicPlayer.Shared;

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
