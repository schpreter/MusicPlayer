using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MusicPlayer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase selectedViewModel;

    private readonly HomeContentViewModel homeContentViewModel;
    private readonly PlaylistsViewModel playlistsViewModel;
    private readonly ArtistsViewModel artistsViewModel;
    private readonly AlbumsViewModel albumsViewModel;
    private readonly GenresViewModel genresViewModel;

    public MainViewModel() { }
    public MainViewModel(HomeContentViewModel homeContent, PlaylistsViewModel playlistsView, ArtistsViewModel artistsView, AlbumsViewModel albumsView, GenresViewModel genresView)
    {
        this.homeContentViewModel = homeContent;
        this.playlistsViewModel = playlistsView;
        this.artistsViewModel = artistsView;
        this.albumsViewModel = albumsView;
        this.genresViewModel = genresView;
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
