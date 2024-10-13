using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Models;
using System.Collections.ObjectModel;


namespace MusicPlayer.Shared
{
    public partial class SharedProperties : ObservableObject
    {
        public ObservableCollection<SongItem> MusicFiles { get; set; }

        [ObservableProperty]
        public SongItem selectedSongListItem;

        [ObservableProperty]
        public int selectedSongIndex;

        //Const for now, later user will beable to choose their own folder path
        [ObservableProperty]
        public string sourceFolderPath = "C:\\Users\\HV387FL\\School\\Szakdoga\\Musics";
        public SongItem CurrentPlayingSong { get; set; }
        public AuthorizationTokenData AuthData;

    }
}
