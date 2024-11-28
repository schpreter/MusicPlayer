using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using DialogHostAvalonia;
using MusicPlayer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib.Id3v2;

namespace MusicPlayer.ViewModels
{
    /// <summary>
    /// Parent class for all the different category ViewModels.
    /// </summary>
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
        /// <summary>
        /// Based on the implementation, removes a song from a given category
        /// </summary>
        /// <param name="song">The song to remove from the category</param>
        protected abstract void RemoveSong(SongItem song);
        /// <summary>
        /// Based on the implementation, adds a song tz a given category
        /// </summary>
        /// <param name="song">The song to add to the category</param>
        protected abstract void AddSong(SongItem song);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>The name of the class that made the call.</returns>
        protected abstract string GetCategory();
        /// <summary>
        /// Lists the songs in the requested category
        /// </summary>
        /// <param name="category">The category, in which the songs should be shown</param>
        public abstract void ShowSongsInCategory(object category);

        //Single song removal from the given category
        /// <summary>
        /// Used for singular song removal, removes the song based on the parameter.
        /// </summary>
        /// <param name="song">The song that needs to be removed</param>
        public abstract void RemoveSingleSong(object song);
        /// <summary>
        /// Adds the selected songs to the currently active category.
        /// </summary>
        public abstract void AddSelectedSongs();
        /// <summary>
        /// Removes the selected songs from the currently active category.
        /// </summary>
        public void RemoveSelectedSongs() { ModifySelectedSongs(true); }

        /// <summary>
        /// Shows the home page of the selected category, listing all sub-categories.
        /// </summary>
        public void ShowHome()
        {
            ShowSongSelectionList = false;
            ShowSongs = false;
            ShowCategoryHome = true;
            UnselectListItems();
            Properties.SongsByCategory.Clear();
        }
        /// <summary>
        /// Shows the view for selecting songs, based on the provided parameter.
        /// </summary>
        /// <param name="selectionType">The type of modification we want to make. In case it equals "Add" it will be addition. Otherwise removal.</param>
        public void ShowSelection(object selectionType)
        {
            SelectionIsAdd = (string)selectionType == "Add";
            ShowSongSelectionList = true;
            ShowSongs = false;
            ShowCategoryHome = false;

        }
        /// <summary>
        /// Handles adding a new sub-category to the current category.
        /// </summary>
        public void AddNewCategory()
        {
            SelectedCategory = null;
            ShowSelection("Add");
        }
        /// <summary>
        /// Modifies the selected songs based on the provided parameter.
        /// </summary>
        /// <param name="isRemove">If <c>true</c> songs will be removed, if <c>false</c> or not provided they will be added.</param>
        public virtual void ModifySelectedSongs(bool isRemove = false)
        {
            if (string.IsNullOrEmpty(SelectedCategory))
            {
                return;
            }
            var selectedSongs = Properties.MusicFiles.Where(x => x.IsSelected);
            //First, change the category that is stored inside the application
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
            //Then based on the changed values, save the modifications to the file
            ModifyFiles(selectedSongs);
        }

        /// <summary>
        /// When modifications happen, updates the bound collection accordingly.
        /// </summary>
        /// <param name="filtered">The songs that should be shown.</param>
        protected void UpdateSongCategory(HashSet<SongItem> filtered)
        {
            //Observable Collection only refreshes UI upon add/remove full reinit operations
            //There is also no built in method for HashSet to ObsevableCollection, could implement in the future tho
            var selectedSong = Properties.SelectedSong;
            SongsByCategory.Clear();
            foreach (var item in filtered)
            {
                SongsByCategory.Add(item);
            }
            //Setting the filtered list in the properties, whivh the navigation can use
            Properties.SongsByCategory = GetItemsForCategory(SelectedCategory);
            Properties.SelectedSong = selectedSong;

            ShowSongs = true;
            ShowCategoryHome = false;
        }

        /// <summary>
        /// When modifications happen, updates the category itself.
        /// </summary>
        /// <param name="keys">The names of all the sub-categories.</param>
        protected void RefreshCategory(HashSet<string> keys)
        {
            HashSet<KeyValuePair<string, Bitmap>> groupedCollection = new HashSet<KeyValuePair<string, Bitmap>>();
            foreach (var key in keys)
            {
                ObservableCollection<SongItem> items = GetItemsForCategory(key);

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
        /// <summary>
        /// Lists all the songs in the sub-category based on the parameter.
        /// </summary>
        /// <param name="key">The requested sub-category</param>
        /// <returns>An <c>ObservableCollection</c> containing the filtered songs.</returns>
        private ObservableCollection<SongItem> GetItemsForCategory(string key)
        {
            switch (GetCategory())
            {
                case nameof(ArtistsViewModel):
                    {
                        return new ObservableCollection<SongItem>(Properties.MusicFiles.Where(x => x.Artists.Contains(key)));
                    }
                case nameof(AlbumsViewModel):
                    {
                        return new ObservableCollection<SongItem>(Properties.MusicFiles.Where(x => x.Album == key));
                    }
                case nameof(GenresViewModel):
                    {
                        return new ObservableCollection<SongItem>(Properties.MusicFiles.Where(x => x.Genres.Contains(key)));
                    }
                case nameof(PlaylistsViewModel):
                    {
                        return new ObservableCollection<SongItem>(Properties.MusicFiles.Where(x => x.PlayLists.Contains(key)));
                    }
                default: return new ObservableCollection<SongItem>();
            }
        }
        /// <summary>
        /// Handles toggling the new input modal.
        /// </summary>
        /// <param name="category">The name of the category, which we want to add a new sub-category into.</param>
        /// <returns>An awaitable <c>Task</c></returns>
        protected async Task ToggleCategoryInputModal(string category)
        {
            if (SelectedCategory == null)
            {
                NewCategoryInputViewModel.Title = $"New {category}:";
                NewCategoryInputViewModel.Description = $"Enter your new {category}.";
                SelectedCategory = (string)await DialogHost.Show(NewCategoryInputViewModel);
            }

        }
        /// <summary>
        /// Stores the modifications made to the <c>SongItem</c> object into it's respective file.
        /// </summary>
        /// <param name="song">The song that needs to be modified</param>
        protected void ModifyFile(SongItem song)
        {
            TagLib.File tagLibFile;
            try
            {
                tagLibFile = TagLib.File.Create(song.FilePath);
            }
            catch (Exception ex)
            {
                //Maybe also notify the user of funky behavior
                return;
            }
            switch (GetCategory())
            {
                case nameof(GenresViewModel):
                    {
                        tagLibFile.Tag.Genres = song.Genres.ToArray();
                        break;
                    }
                case nameof(ArtistsViewModel):
                    {
                        tagLibFile.Tag.Performers = song.Artists.ToArray();
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
        /// <summary>
        /// Modifes several files, based on the provided parameter.
        /// </summary>
        /// <param name="songs">The songs that need to be modified.</param>
        protected virtual void ModifyFiles(IEnumerable songs)
        {
            foreach (SongItem song in songs)
            {
                ModifyFile(song);
            }
            RefreshContent();
            ShowSongsInCategory(SelectedCategory);
        }
        /// <summary>
        /// Handles storing the playlist tag based on mime type.
        /// </summary>
        /// <param name="song">The song as stored inside the application.</param>
        /// <param name="tagLibFile">The Taglib.File created from the song.</param>
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
        /// <summary>
        /// Unselects all selectable list items.
        /// </summary>
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
