using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Data;
using MusicPlayer.Shared;
using MusicPlayer.Views;
using System.Linq;
using TagLib;

namespace MusicPlayer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase selectedViewModel;

    [ObservableProperty]
    private HomeContentViewModel homeContentViewModel;
    private readonly PlaylistsViewModel playlistsViewModel;
    private readonly ArtistsViewModel artistsViewModel;
    private readonly AlbumsViewModel albumsViewModel;
    private readonly GenresViewModel genresViewModel;
    #region TO REMOVE
    private const string mp3AndFlacFolderPathTest = "C:\\Users\\HV387FL\\School\\Szakdoga\\Musics";
    private const string testFolderPath = "C:\\Users\\HV387FL\\School\\Szakdoga\\Musics\\steins-gate-anime-complete-soundtrack\\Disc 1";
    #endregion

    private readonly MainWindow mainWindow;

    [ObservableProperty]
    public MusicNavigationViewModel musicNavigation;

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
        this.musicNavigation = musicNavigationView;
        this.mainWindow = mainWindow;
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
        if (selectedFolder != null)
        {
            Properties.MusicFiles = MusicFileCollector.CollectFilesFromFolder(selectedFolder.First().TryGetLocalPath());
        }
    }

}
