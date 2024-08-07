using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Data;
using MusicPlayer.Shared;
using MusicPlayer.Views;
using System.Linq;

namespace MusicPlayer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase selectedViewModel;
    private SharedProperties Properties;

    [ObservableProperty]
    private HomeContentViewModel homeContentViewModel;
    private readonly PlaylistsViewModel playlistsViewModel;
    private readonly ArtistsViewModel artistsViewModel;
    private readonly AlbumsViewModel albumsViewModel;
    private readonly GenresViewModel genresViewModel;
    private const string mp3AndFlacFolderPathTest = "C:\\Users\\HV387FL\\School\\Szakdoga\\Musics";

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
        this.HomeContentViewModel = homeContent;
        this.playlistsViewModel = playlistsView;
        this.artistsViewModel = artistsView;
        this.albumsViewModel = albumsView;
        this.genresViewModel = genresView;
        this.musicNavigation = musicNavigationView;
        this.mainWindow = mainWindow;
        this.Properties = sharedProperties;
        SelectedViewModel = HomeContentViewModel;
    }
    #region ViewModel Switching
    public void ShowHomeContent()
    {
        SelectedViewModel = HomeContentViewModel;
    }
    public void ShowPlaylistsContent()
    {
        SelectedViewModel = playlistsViewModel;
    }
    public void ShowArtistsContent()
    {
        SelectedViewModel = artistsViewModel;
    }
    public void ShowAlbumsContent()
    {
        SelectedViewModel = albumsViewModel;
    }
    public void ShowGenresContent()
    {
        SelectedViewModel = genresViewModel;
    }
    #endregion
    public async void SetInputFolder()
    {
        var selectedFolder = await TopLevel.GetTopLevel(mainWindow).StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions { AllowMultiple = false, Title = "Test" });
        if (selectedFolder != null)
        {
            Properties.MusicFiles = MusicFileCollector.CollectFilesFromFolder(selectedFolder.First().TryGetLocalPath());
            //if (MusicNavigation.MediaPlayer.IsPlaying) MusicNavigation.MediaPlayer.Stop(); 
        }
    }

}
