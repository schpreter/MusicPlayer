using MusicPlayer.Shared;

namespace MusicPlayer.ViewModels
{
    /// <summary>
    /// Class which serves as a base for displaying the current playing song.
    /// </summary>
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
