using System.Collections.ObjectModel;
using Avalonia.Controls;
using MusicPlayer.Models;
using MusicPlayer.Models.Abstracts;


namespace MusicPlayer.ViewModels
{
    public partial class ArtistsViewModel : GenericCoverDisplay
    {
        public ArtistsViewModel()
        {
            ItemCollection = new ObservableCollection<GenericDisplayItem>
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
