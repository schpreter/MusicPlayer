using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace MusicPlayer.Shared
{
    public partial class SharedProperties : ObservableObject
    {
        public ObservableCollection<SongItem> MusicFiles { get; set; }
        public List<SongItem> SongsByCategory { get; set; }
        public string Category { get; set; }
        public string CategoryName { get; set; }
        [ObservableProperty]
        public SongItem selectedSong;

        [ObservableProperty]
        public int selectedSongIndex;

        //Const for now, later user will beable to choose their own folder path
        [ObservableProperty]
        public string sourceFolderPath = "C:\\Users\\HV387FL\\School\\Szakdoga\\Musics";
        public SongItem CurrentSong { get; set; }
        public AuthorizationTokenData AuthData;

    }
}
