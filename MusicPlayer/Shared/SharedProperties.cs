using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Models;
using MusicPlayer.ViewModels;
using System.Collections.ObjectModel;


namespace MusicPlayer.Shared
{
    public partial class SharedProperties : ViewModelBase
    {
        private ObservableCollection<SongListItem> musicFiles;
        public ObservableCollection<SongListItem> MusicFiles
        {
            get { return musicFiles; }
            set { SetProperty(ref musicFiles, value); }
        }

        [ObservableProperty]
        public SongListItem selectedSongListItem;

        [ObservableProperty]
        public int selectedSongIndex;

        public SongListItem CurrentPlayingSong { get; set; }

        [ObservableProperty]
        public string sourceFolderPath;



    }
}
