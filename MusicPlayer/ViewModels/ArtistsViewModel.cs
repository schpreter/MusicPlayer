using MusicPlayer.Interfaces;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace MusicPlayer.ViewModels
{
    public partial class ArtistsViewModel : UnifiedCategoryViewModel
    {
        public ArtistsViewModel()
        {

        }
        public ArtistsViewModel(SharedProperties props, NewCategoryInputViewModel newCategoryInput, ITaglLibFactory taglLibFactory)
        {
            Properties = props;
            NewCategoryInputViewModel = newCategoryInput;
            this.taglLibFactory = taglLibFactory;
        }
        public override void RefreshContent()
        {
            var ArtistsSet = Properties.MusicFiles.SelectMany(x => x.Artists).ToHashSet();
            RefreshCategory(ArtistsSet);


        }
        public override void ShowSongsInCategory(object genre)
        {
            SelectedCategory = (string)genre;
            HashSet<SongItem> filtered = Properties.MusicFiles.Where(x => x.Artists.Contains(SelectedCategory)).ToHashSet();
            UpdateSongCategory(filtered);
        }


        public override async void AddSelectedSongs()
        {
            await ToggleCategoryInputModal("artist");
            ModifySelected();
        }

        public override string ToString()
        {
            return "Artists";
        }

        public override void RemoveSong(object song)
        {
            SongItem item = (SongItem)song;
            if (item.Artists.Remove(SelectedCategory))
            {
                RemoveSingleTag(item);
            }
        }

        protected override void RemoveCurrentSong(SongItem song)
        {
            song.Artists.Remove(SelectedCategory);
        }

        protected override void AddCurrentSong(SongItem song)
        {
            if (!song.Artists.Contains(SelectedCategory))
            {
                song.Artists.Add(SelectedCategory);
            }
        }

        protected override string GetCategory()
        {
            return nameof(ArtistsViewModel);
        }
    }
}
