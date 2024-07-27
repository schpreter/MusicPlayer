using System.Collections.ObjectModel;
using MusicPlayer.Models;


namespace MusicPlayer.ViewModels
{
    public partial class ArtistsViewModel : ViewModelBase
    {
        private ObservableCollection<ArtistItem> artists;

        public ObservableCollection<ArtistItem> Artists
        {
            get { return artists; }
            set { SetProperty(ref artists, value); }
        }
        public ArtistsViewModel()
        {
            Artists = new ObservableCollection<ArtistItem>
            {
                new ArtistItem("Ren", "avares://MusicPlayer/Assets/ren.jpg"),
                new ArtistItem("Metallica", "avares://MusicPlayer/Assets/metallica.jpg"),
                new ArtistItem("Ghost", "avares://MusicPlayer/Assets/ghost.jpg"),
                new ArtistItem("Slipknot", "avares://MusicPlayer/Assets/slipknot.jpg"),
                new ArtistItem("Ren", "avares://MusicPlayer/Assets/ren.jpg"),
                new ArtistItem("Metallica", "avares://MusicPlayer/Assets/metallica.jpg"),
                new ArtistItem("Ghost", "avares://MusicPlayer/Assets/ghost.jpg"),
                new ArtistItem("Slipknot", "avares://MusicPlayer/Assets/slipknot.jpg")
            };
        }
    }
}
