﻿using MusicPlayer.Models;
using System.Collections.ObjectModel;
using System.Linq;
using MusicPlayer.Shared;

namespace MusicPlayer.ViewModels
{
    public partial class GenresViewModel : GenericCoverDisplay
    {


        public GenresViewModel(SharedProperties props)
        {
            ItemCollection = new ObservableCollection<UnifiedDisplayItem>();
            SongsByCategory = new ObservableCollection<SongListItem> { };
            Properties = props;
        }
        public override void RefreshContent()
        {
            var Genres = Properties.MusicFiles.SelectMany(x => x.Genres).ToHashSet();
            RefreshCategory(Genres);

        }
        public override void ShowSongsInCategory(object genre)
        {
            SelectedCategory = (string)genre;
            var filtered = Properties.MusicFiles.Where(x => x.Genres.Contains(SelectedCategory));
            UpdateSongCategory(filtered);
        }

    }
}
