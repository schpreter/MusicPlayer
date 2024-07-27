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

    public MainViewModel() { }
    public MainViewModel(HomeContentViewModel homeContent, PlaylistsViewModel playlistsView, ArtistsViewModel artistsView)
    {
        this.homeContentViewModel = homeContent;
        this.playlistsViewModel = playlistsView;
        this.artistsViewModel = artistsView;
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


}
