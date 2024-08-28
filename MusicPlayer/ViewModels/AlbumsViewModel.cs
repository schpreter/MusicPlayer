using Avalonia.Controls;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Shared;
using CommunityToolkit.Mvvm.ComponentModel;
using TagLib;

namespace MusicPlayer.ViewModels
{
    public partial class AlbumsViewModel : GenericCoverDisplay
    {
        public AlbumsViewModel(SharedProperties props)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByCategory = new ObservableCollection<SongListItem>();
            Properties = props;
        }

        public override void RefreshContent()
        {
            HashSet<string> Albums = Properties.MusicFiles.Select(x => x.Album).ToHashSet();
            RefreshCategory(Albums);

        }
        public override void ShowSongsInCategory(object album)
        {
            SelectedCategory = (string)album;
            var filtered = Properties.MusicFiles.Where(x => x.Album == SelectedCategory);
            UpdateSongCategory(filtered);
        }
    }
}
