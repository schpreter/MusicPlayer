using MusicPlayer.Models.Abstracts;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Shared;

namespace MusicPlayer.ViewModels
{
    public partial class GenresViewModel : GenericCoverDisplay
    {
        public GenresViewModel(SharedProperties props)
        {
            ItemCollection = new ObservableCollection<GenericDisplayItem>();
            Properties = props;
        }
        public override void RefreshContent()
        {
            var Genres = Properties.MusicFiles.SelectMany(x => x.SongMetaData.Genres).ToHashSet();
            foreach (var item in Genres)
            {
                ItemCollection.Add(new GenreItem(item));
            }
        }
    }
}
