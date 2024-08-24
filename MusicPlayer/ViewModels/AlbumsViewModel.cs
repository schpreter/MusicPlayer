using Avalonia.Controls;
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
    public partial class AlbumsViewModel : GenericCoverDisplay
    {
        public AlbumsViewModel(SharedProperties props)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            Properties = props;

        }

        public override void RefreshContent()
        {
            List<string> Albums = Properties.MusicFiles.Select(x => x.Album).ToList();
            foreach (string item in Albums)
            {
                ItemCollection.Add(new UnifiedDisplayItem(item));
            }
        }
    }
}
