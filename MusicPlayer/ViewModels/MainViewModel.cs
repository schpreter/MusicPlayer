using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Data;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using MusicPlayer.Views;
using System;
using System.IO;
using System.Linq;
using TagLib;

namespace MusicPlayer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase selectedViewModel;

    [ObservableProperty]
    private  HomeContentViewModel homeContentViewModel;
    private readonly PlaylistsViewModel playlistsViewModel;
    private readonly ArtistsViewModel artistsViewModel;
    private readonly AlbumsViewModel albumsViewModel;
    private readonly GenresViewModel genresViewModel;
    #region TO REMOVE
    private const string testFolderPath = "C:\\Users\\HV387FL\\School\\Szakdoga\\Musics";
    #endregion

    private readonly MainWindow mainWindow;

    [ObservableProperty]
    private MusicNavigationViewModel musicNavigation;

    public MainViewModel() { }
    public MainViewModel(HomeContentViewModel homeContent,
        PlaylistsViewModel playlistsView,
        ArtistsViewModel artistsView,
        AlbumsViewModel albumsView,
        GenresViewModel genresView,
        MusicNavigationViewModel musicNavigationView,
        MainWindow mainWindow,
        SharedProperties sharedProperties)
    {
        this.Properties = sharedProperties;
        this.HomeContentViewModel = homeContent;
        Init();

        this.playlistsViewModel = playlistsView;
        this.artistsViewModel = artistsView;
        this.albumsViewModel = albumsView;
        this.genresViewModel = genresView;
        this.mainWindow = mainWindow;
        this.musicNavigation = musicNavigationView;
    }
    private void Init()
    {
        SelectedViewModel = HomeContentViewModel;
        //Possibly remove in the end, user can input the source folder themselves
        Properties.MusicFiles = MusicFileCollector.CollectFilesFromFolder(testFolderPath);

    }
    #region ViewModel Switching
    public void ShowContent(string view)
    {
        switch (view)
        {
            case "HOME":
                SelectedViewModel = HomeContentViewModel;
                break;
            case "PLAYLISTS":
                SelectedViewModel = playlistsViewModel;
                break;
            case "ARTISTS":
                SelectedViewModel = artistsViewModel;
                break;
            case "ALBUMS":
                SelectedViewModel = albumsViewModel;
                break;
            case "GENRES":
                SelectedViewModel = genresViewModel;
                break;
            default:
                break;
        }
        SelectedViewModel.RefreshContent();

    }
    #endregion
    public async void SetInputFolder()
    {
        var selectedFolder = await TopLevel.GetTopLevel(mainWindow).StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions { AllowMultiple = false, Title = "Select Input Folder" });
        if (selectedFolder.Count > 0)
        {
            Properties.MusicFiles = MusicFileCollector.CollectFilesFromFolder(selectedFolder.First().TryGetLocalPath());
        }
    }

    public void ModifyTags()
    {
        string[] allowedExtensions = new[] { ".mp3" };
        var listOfFilesInFolder = Directory.GetFiles(testFolderPath).Where(fil => allowedExtensions.Any(fil.ToLower().EndsWith));
        var relativePaths = listOfFilesInFolder.Select(path => Path.GetFileName(path)).ToList();
        foreach (string item in listOfFilesInFolder)
        {
            TagLib.File tagLibFile = TagLib.File.Create(item);
            //SongListItem songItem = new SongListItem
            //{
            //    Album = tagLibFile.Tag.Album,
            //    Title = tagLibFile.Tag.Title == null ? Path.GetFileName(item).Split('.').First() : tagLibFile.Tag.Title,
            //    Artists = tagLibFile.Tag.Performers.ToList(),
            //    Artists_conc = tagLibFile.Tag.JoinedPerformers,
            //    Genres = tagLibFile.Tag.Genres.ToList(),
            //    Year = (int)tagLibFile.Tag.Year,
            //    Duration = TimeSpan.FromSeconds(tagLibFile.Properties.Duration.TotalSeconds),
            //    FilePath = tagLibFile.Name

            //};
            tagLibFile.Tag.Album = "Test Album - " + Random.Shared.NextInt64();
            tagLibFile.Save();
        }
        Properties.MusicFiles = MusicFileCollector.CollectFilesFromFolder(testFolderPath);

    }

}
