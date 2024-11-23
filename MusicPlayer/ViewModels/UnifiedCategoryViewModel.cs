using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using DialogHostAvalonia;
using MusicPlayer.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TagLib.Id3v2;

namespace MusicPlayer.ViewModels
{
    public abstract partial class UnifiedCategoryViewModel : ViewModelBase
    {

        public NewCategoryInputViewModel NewCategoryInputViewModel { get; set; }
        public ObservableCollection<SongItem> SongsByCategory { get; set; } = new ObservableCollection<SongItem>();
        public ObservableCollection<UnifiedDisplayItem> ItemCollection { get; set; } = new ObservableCollection<UnifiedDisplayItem>();

        [ObservableProperty]
        public string selectedCategory;

        [ObservableProperty]
        public bool showSongs = false;

        [ObservableProperty]
        public bool showSongSelectionList = false;

        [ObservableProperty]
        public bool showCategoryHome = true;

        [ObservableProperty]
        public bool newCategoryDialogOpen = false;

        [ObservableProperty]
        public bool selectionIsAdd;

        protected abstract void RemoveSong(SongItem song);
        protected abstract void AddSong(SongItem song);
        protected abstract string GetCategory();
        public abstract void ShowSongsInCategory(object category);

        //Single song removal from the given category
        public abstract void RemoveSingleSong(object song);
        public abstract void AddSelectedSongs();
        public void RemoveSelectedSongs() { ModifySelectedSongs(true); }


        public void ShowHome()
        {
            ShowSongSelectionList = false;
            ShowSongs = false;
            ShowCategoryHome = true;
            UnselectListItems();
            Properties.SongsByCategory.Clear();
        }
        public void ShowSelection(object selectionType)
        {
            SelectionIsAdd = (string)selectionType == "Add";
            ShowSongSelectionList = true;
            ShowSongs = false;
            ShowCategoryHome = false;

        }
        public void AddNewCategory()
        {
            SelectedCategory = null;
            ShowSelection("Add");
        }
        public virtual void ModifySelectedSongs(bool isRemove = false)
        {
            if (string.IsNullOrEmpty(SelectedCategory))
            {
                return;
            }

            var selectedSongs = Properties.MusicFiles.Where(x => x.IsSelected);
            //First we change the category that is stored inside the application
            if (isRemove)
            {
                foreach (var song in selectedSongs)
                {
                    RemoveSong(song);
                }
            }
            else
            {
                foreach (var song in selectedSongs)
                {
                    AddSong(song);

                }
            }

            //Then based on the changed values we save the modifications to the file
            ModifyFiles(selectedSongs);

        }


        protected void UpdateSongCategory(HashSet<SongItem> filtered)
        {
            //Observable Collection only refreshes UI upon add/remove full reinit operations
            //There is also no built in method for HashSet to ObsevableCollection, could implement in the future tho
            SongsByCategory.Clear();
            foreach (var item in filtered)
            {
                SongsByCategory.Add(item);
            }
            //Setting the filtered list in the properties, whivh the navigation can use
            Properties.SongsByCategory = GetItemsForCategory(SelectedCategory);

            ShowSongs = true;
            ShowCategoryHome = false;
        }
        protected void RefreshCategory(HashSet<string> keys)
        {
            HashSet<KeyValuePair<string, Bitmap>> groupedCollection = new HashSet<KeyValuePair<string, Bitmap>>();
            foreach (var key in keys)
            {
                List<SongItem> items = GetItemsForCategory(key);

                if (items.Any())
                {
                    groupedCollection.Add(KeyValuePair.Create(key, items.ToList().First().FirstImage));
                }
            }

            ItemCollection.Clear();
            foreach (var item in groupedCollection)
            {
                ItemCollection.Add(new UnifiedDisplayItem(item.Key, item.Value));
            }
            ShowHome();
        }
        private List<SongItem> GetItemsForCategory(string key)
        {
            List<SongItem> res = new List<SongItem>();
            switch (GetCategory())
            {
                case nameof(ArtistsViewModel):
                    {
                        res = Properties.MusicFiles.Where(x => x.Artists.Contains(key)).ToList();
                        break;
                    }
                case nameof(AlbumsViewModel):
                    {
                        res = Properties.MusicFiles.Where(x => x.Album == key).ToList();
                        break;
                    }
                case nameof(GenresViewModel):
                    {
                        res = Properties.MusicFiles.Where(x => x.Genres.Contains(key)).ToList();
                        break;
                    }
                case nameof(PlaylistsViewModel):
                    {
                        res = Properties.MusicFiles.Where(x => x.PlayLists.Contains(key)).ToList();
                        break;
                    }
            }
            return res;
        }

        protected async Task ToggleCategoryInputModal(string category)
        {
            //First, if the selected category is null, we must prompt the user to select a category
            //In case of new category this is always the case
            if (SelectedCategory == null)
            {
                NewCategoryInputViewModel.Title = $"New {category}:";
                NewCategoryInputViewModel.Description = $"Enter your new {category}.";
                SelectedCategory = (string)await DialogHost.Show(NewCategoryInputViewModel);
            }

        }
        protected void ModifyFile(SongItem song)
        {
            TagLib.File tagLibFile;
            try
            {
                tagLibFile = TagLib.File.Create(song.FilePath);
            }
            catch
            {
                //Maybe also notify the user of funky behavior
                return;
            }

            switch (GetCategory())
            {
                case nameof(GenresViewModel):
                    {
                        tagLibFile.Tag.Genres = song.Genres.ToArray();
                        //song.Genres = tagLibFile.Tag.Genres.ToList();
                        break;
                    }
                case nameof(ArtistsViewModel):
                    {
                        tagLibFile.Tag.Performers = song.Artists.ToArray();
                        //song.Artists_conc = tagLibFile.Tag.JoinedPerformers;
                        break;
                    }
                case nameof(AlbumsViewModel):
                    {
                        tagLibFile.Tag.Album = song.Album;
                        break;
                    }
                //This is where the file format matters, just like during parsing
                case nameof(PlaylistsViewModel):
                    {
                        ModifyPlaylistTag(song, tagLibFile);
                        break;
                    }
                default:
                    break;
            }
            tagLibFile.Save();

        }

        protected virtual void ModifyFiles(IEnumerable songs)
        {
            foreach (SongItem song in songs)
            {
                ModifyFile(song);
            }
            RefreshContent();
            ShowSongsInCategory(SelectedCategory);
        }

        private void ModifyPlaylistTag(SongItem song, TagLib.File tagLibFile)
        {
            switch (tagLibFile.MimeType)
            {
                case "taglib/mp3":
                    {
                        TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tagLibFile.GetTag(TagLib.TagTypes.Id3v2, true);
                        PrivateFrame pFrame = PrivateFrame.Get(tag, "Playlists", true);

                        var cleaned = song.PlayLists;
                        cleaned.ForEach(x => x.Replace(";", @"\;"));

                        pFrame.PrivateData = Encoding.Unicode.GetBytes(string.Join(';', cleaned));
                        break;
                    }
                default:
                    {
                        var tag = (TagLib.Ogg.XiphComment)tagLibFile.GetTag(TagLib.TagTypes.Xiph, true);
                        tag.SetField("Playlists", song.PlayLists.ToArray());
                        break;
                    }
            }
        }

        //private void AddPlaylistTag(List<string> playlists, TagLib.File tagLibFile)
        //{
        //    switch (tagLibFile.MimeType)
        //    {
        //        case "taglib/mp3":
        //            {
        //                TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tagLibFile.GetTag(TagLib.TagTypes.Id3v2, true);
        //                PrivateFrame pFrame = PrivateFrame.Get(tag, "Playlists", true);

        //                List<string> list = new List<string>();

        //                //If there is actual data in the private frame, parse it
        //                if (pFrame.PrivateData != null)
        //                {
        //                    string data = Encoding.Unicode.GetString(pFrame.PrivateData.Data);
        //                    if (data != null)
        //                    {
        //                        list = Regex.Split(data, @"(?<!\\);").ToList();
        //                    }
        //                }

        //                //Clean user input
        //                var cleaned = playlists;
        //                cleaned.ForEach(x => x.Replace(";", @"\;"));

        //                pFrame.PrivateData = Encoding.Unicode.GetBytes(string.Join(';', list.Union(cleaned)));
        //                break;
        //            }
        //        default:
        //            {
        //                var tag = (TagLib.Ogg.XiphComment)tagLibFile.GetTag(TagLib.TagTypes.Xiph, true);
        //                //Using union get an IEnumerable to distinct playlist names -> playlist names hsould be unique therefore
        //                var filtered = tag.GetField("Playlists").Union(playlists);

        //                tag.SetField("Playlists", filtered.ToArray());
        //                break;
        //            }
        //    }
        //}

        private void UnselectListItems()
        {
            foreach (SongItem item in Properties.MusicFiles)
            {
                if (item.IsSelected)
                    item.IsSelected = false;
            }
        }

    }
}
