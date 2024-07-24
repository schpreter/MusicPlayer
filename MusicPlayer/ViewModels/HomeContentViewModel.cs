using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Models;
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

        private ObservableCollection<SongListItem> musicFiles;

        public ObservableCollection<SongListItem> MusicFiles
        {
            get { return musicFiles; }
            set { SetProperty(ref musicFiles, value); }
        }

        public HomeContentViewModel()
        {
            MusicFiles = new ObservableCollection<SongListItem> 
            {
                new SongListItem("Metallica", "Enter Sandman"),
                new SongListItem("Ghost", "Year Zero"),
                new SongListItem("Ren", "Animal Flow"),
                new SongListItem("Slipknot", "Unsainted"),
            };

        }
    }
}
