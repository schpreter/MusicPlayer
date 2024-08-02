using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Data;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels
{
    public partial class HomeContentViewModel : ViewModelBase
    {
        private const string testFolderPath = "C:\\Users\\HV387FL\\School\\Szakdoga\\Musics\\steins-gate-anime-complete-soundtrack\\Disc 1";
        private ObservableCollection<SongListItem> musicFiles;
        [ObservableProperty]
        private SharedProperties properties;

        public ObservableCollection<SongListItem> MusicFiles
        {
            get { return musicFiles; }
            set { SetProperty(ref musicFiles, value); }
        }

        public HomeContentViewModel(SharedProperties props)
        {
            MusicFiles = MusicFileCollector.CollectFilesFromFolder(testFolderPath);
            Properties = props;
        }


    }
}
