using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Data;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using MusicPlayer.Views;
using System.Linq;

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
    private readonly ControlWidget control;
    #region TO REMOVE
    private const string testFolderPath = "C:\\Users\\HV387FL\\School\\Szakdoga\\Musics";
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
        ControlWidget controlWidget,
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
        this.control = controlWidget;
        this.mainWindow = mainWindow;

        PixelRect screen = this.mainWindow.Screens.Primary.WorkingArea;

        control.Position = new PixelPoint((int)(screen.BottomRight.X), (int)(screen.BottomRight.Y));
        control.Show();
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

}
