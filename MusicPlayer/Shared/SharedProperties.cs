using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Models;
using MusicPlayer.ViewModels;


namespace MusicPlayer.Shared
{
    public partial class SharedProperties : ViewModelBase
    {
        [ObservableProperty]
        public SongListItem selectedSongListItem;
    }
}
