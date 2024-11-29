using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using MusicPlayer.Models;
using System;
using System.Collections.ObjectModel;


namespace MusicPlayer.Shared
{
    /// <summary>
    /// Data sharing class, it's purpose is to avoid unnecessary overcomplication.
    /// Fields that are used by several ViewModels are stored here.
    /// </summary>
    public partial class SharedProperties : ObservableObject
    {
        public ObservableCollection<SongItem> MusicFiles { get; set; } = new ObservableCollection<SongItem>();
        public ObservableCollection<SongItem> SongsByCategory { get; set; } = new ObservableCollection<SongItem>();
        public string Category { get; set; }
        public string CategoryName { get; set; }
        [ObservableProperty]
        public SongItem selectedSong;

        [ObservableProperty]
        public string sourceFolderPath = GetRootFolder();

        [ObservableProperty]
        public SongItem playingSong;
        public AuthorizationTokenData AuthData;

        private static string GetRootFolder()
        {
            bool isWindows = OperatingSystem.IsWindows();
            string basePath = new ConfigurationBuilder().AddJsonFile("appsettings.json").
                Build().GetSection("SOURCE_FOLDERS")[isWindows ? "WIN" : "LINUX"];

            if (string.IsNullOrEmpty(basePath))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            }

            return basePath;
        }


    }
}
