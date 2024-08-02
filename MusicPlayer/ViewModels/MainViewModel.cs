using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using MusicPlayer.Data;
using MusicPlayer.Models;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MusicPlayer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase selectedViewModel;

    [ObservableProperty]
    public HomeContentViewModel homeContentViewModel;
    private readonly PlaylistsViewModel playlistsViewModel;
    private readonly ArtistsViewModel artistsViewModel;
    private readonly AlbumsViewModel albumsViewModel;
    private readonly GenresViewModel genresViewModel;
    private const string mp3AndFlacFolderPathTest = "C:\\Users\\HV387FL\\School\\Szakdoga\\Musics";


    [ObservableProperty]
    public MusicNavigationViewModel musicNavigation;

    public MainViewModel() { }
    public MainViewModel(HomeContentViewModel homeContent,
        PlaylistsViewModel playlistsView,
        ArtistsViewModel artistsView,
        AlbumsViewModel albumsView,
        GenresViewModel genresView,
        MusicNavigationViewModel musicNavigationView)
    {
        this.HomeContentViewModel = homeContent;
        this.playlistsViewModel = playlistsView;
        this.artistsViewModel = artistsView;
        this.albumsViewModel = albumsView;
        this.genresViewModel = genresView;
        this.musicNavigation = musicNavigationView;
        SelectedViewModel = homeContentViewModel;
    }

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

    public void SetInputFolder()
    {
        HomeContentViewModel.MusicFiles = MusicFileCollector.CollectFilesFromFolder(mp3AndFlacFolderPathTest);
    }

}
