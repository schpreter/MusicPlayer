using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MusicPlayer.ViewModels;

public partial class MainViewModel : ViewModelBase, IDisposable
{
    [ObservableProperty]
    private ViewModelBase selectedViewModel;

    private readonly HomeContentViewModel homeContentViewModel;
    private readonly PlaylistsViewModel playlistsViewModel;
    private readonly ArtistsViewModel artistsViewModel;
    private readonly AlbumsViewModel albumsViewModel;
    private readonly GenresViewModel genresViewModel;

    private readonly LibVLC libVlc = new LibVLC();
    public MediaPlayer MediaPlayer { get; }



    public void Play()
    {
        if (Design.IsDesignMode)
        {
            return;
        }

        using Media? media = new Media(libVlc, new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));
        MediaPlayer.Play(media);
    }

    public void Stop()
    {
        MediaPlayer.Stop();
    }

    public void Dispose()
    {
        MediaPlayer?.Dispose();
        libVlc?.Dispose();
    }


    public MainViewModel() { }
    public MainViewModel(HomeContentViewModel homeContent, PlaylistsViewModel playlistsView, ArtistsViewModel artistsView, AlbumsViewModel albumsView, GenresViewModel genresView)
    {
        this.homeContentViewModel = homeContent;
        this.playlistsViewModel = playlistsView;
        this.artistsViewModel = artistsView;
        this.albumsViewModel = albumsView;
        this.genresViewModel = genresView;
        this.MediaPlayer = new MediaPlayer(libVlc);
        SelectedViewModel = homeContentViewModel;
    }

    public void ShowHomeContent()
    {
        SelectedViewModel = homeContentViewModel;
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


}
