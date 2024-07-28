using Avalonia.Controls;
using MusicPlayer.Models.Abstracts;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels
{
    public partial class AlbumsViewModel : GenericCoverDisplay
    {
        public AlbumsViewModel()
        {
            ItemCollection = new ObservableCollection<GenericDisplayItem>
            {
                //TODO: seperate artist name + album maybe
                new AlbumItem("Ren - Violet's Tale", "avares://MusicPlayer/Assets/ren.jpg"),
                new AlbumItem("Metallica - Some album", "avares://MusicPlayer/Assets/metallica.jpg"),
                new AlbumItem("Ghost - some album 2", "avares://MusicPlayer/Assets/ghost.jpg")
            };
        }
    }
}
