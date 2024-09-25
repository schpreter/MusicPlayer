using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Models;
using MusicPlayer.ViewModels;
using System.Collections.ObjectModel;


namespace MusicPlayer.Shared
{
    public partial class SharedProperties : ObservableObject
    {
        private ObservableCollection<SongItem> musicFiles;
        public ObservableCollection<SongItem> MusicFiles
        {
            get { return musicFiles; }
            set { SetProperty(ref musicFiles, value); }
        }

        [ObservableProperty]
        public SongItem selectedSongListItem;

        [ObservableProperty]
        public int selectedSongIndex;

        //Const for now, later user will beable to choose their own folder path
        [ObservableProperty]
        public string sourceFolderPath = "C:\\Users\\HV387FL\\School\\Szakdoga\\Musics";
        public SongItem CurrentPlayingSong { get; set; }

    }
}
