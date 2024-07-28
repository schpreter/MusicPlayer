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
    public partial class GenresViewModel : GenericCoverDisplay
    {
        public GenresViewModel()
        {
            ItemCollection = new ObservableCollection<GenericDisplayItem>
            {
                //TODO: seperate artist name + album maybe
                new GenreItem("Rap", "avares://MusicPlayer/Assets/ren.jpg"),
                new GenreItem("Metal", "avares://MusicPlayer/Assets/metallica.jpg"),
                new GenreItem("Rock", "avares://MusicPlayer/Assets/ghost.jpg")
            };
        }
    }
}
