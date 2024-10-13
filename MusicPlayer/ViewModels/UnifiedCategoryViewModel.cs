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
        protected NewCategoryInputViewModel NewCategoryInputViewModel { get; set; }
        public ObservableCollection<SongItem> SongsByCategory { get; set; }
        public ObservableCollection<UnifiedDisplayItem> ItemCollection { get; set; }

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

        public abstract void ShowSongsInCategory(object category);
        public abstract void AddSelectedSongs();

        public void ShowHome()
        {
            ShowSongSelectionList = false;
            ShowSongs = false;
            ShowCategoryHome = true;
        }
        public void ShowSelection()
        {
            ShowSongSelectionList = true;
            ShowSongs = false;
            ShowCategoryHome = false;

        }
        public void AddNewCategory()
        {
            SelectedCategory = null;
            ShowSelection();
        }

        protected void UpdateSongCategory(HashSet<SongItem> filtered)
        {
            SongsByCategory.Clear();
            foreach (var item in filtered)
            {
                SongsByCategory.Add(item);
            }
            ShowSongs = true;
            ShowCategoryHome = false;
        }
        //TODO: Unselect songs upon submit
        protected void RefreshCategory(HashSet<string> set)
        {
            ItemCollection.Clear();
            foreach (var item in set)
            {
                ItemCollection.Add(new UnifiedDisplayItem(item));
            }
            ShowHome();
        }
        protected async Task ToggleCategoryInputModal(string categoryType)
        {
            //First, if the selected category is null, we must prompt the user to select a category
            if (SelectedCategory == null)
            {
                NewCategoryInputViewModel.Title = $"New {categoryType}:";
                NewCategoryInputViewModel.Description = $"Enter your new {categoryType}.";
                SelectedCategory = (string)await DialogHost.Show(NewCategoryInputViewModel);
            }

        }

        protected void ModifyFiles(IEnumerable songs, string category)
        {
            foreach (SongItem song in songs)
            {
                TagLib.File tagLibFile = TagLib.File.Create(song.FilePath);
                switch (category)
                {
                    case "GENRES":
                        {
                            tagLibFile.Tag.Genres = song.Genres.ToArray();
                            song.Genres = tagLibFile.Tag.Genres.ToList();
                            break;
                        }
                    case "ARTISTS":
                        {
                            tagLibFile.Tag.Album = song.Album;
                            break;
                        }
                    case "ALBUM":
                        {
                            tagLibFile.Tag.Performers = song.Artists.ToArray();
                            song.Artists_conc = tagLibFile.Tag.JoinedPerformers;
                            break;
                        }
                    //This is where the file format matters, just like during parsing
                    case "PLAYLISTS":
                        {
                            ModifyPlaylistTag(song.PlayLists, tagLibFile);
                            break;
                        }
                    default:
                        break;
                }
                tagLibFile.Save();
            }
            RefreshContent();
            ShowHome();
        }

        private void ModifyPlaylistTag(List<string> playlists, TagLib.File tagLibFile)
        {
            switch (tagLibFile.MimeType)
            {
                case "taglib/mp3":
                    {
                        TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tagLibFile.GetTag(TagLib.TagTypes.Id3v2, true);
                        PrivateFrame pFrame = PrivateFrame.Get(tag, "Playlists", true);

                        List<string> list = new List<string>();

                        //If there is actual data in the private frame, parse it
                        if (pFrame.PrivateData != null)
                        {
                            string data = Encoding.Unicode.GetString(pFrame.PrivateData.Data);
                            if (data != null)
                            {
                                list = Regex.Split(data, @"(?<!\\);").ToList();
                            }
                        }

                        //Clean user input
                        var cleaned = playlists;
                        cleaned.ForEach(x => x.Replace(";", @"\;"));

                        pFrame.PrivateData = Encoding.Unicode.GetBytes(string.Join(';', list.Union(cleaned)));
                        break;
                    }
                default:
                    {
                        var tag = (TagLib.Ogg.XiphComment)tagLibFile.GetTag(TagLib.TagTypes.Xiph, true);
                        //Using union get an IEnumerable to distinct playlist names -> playlist names hsould be unique therefore
                        var filtered = tag.GetField("Playlists").Union(playlists);

                        tag.SetField("Playlists", filtered.ToArray());
                        break;
                    }
            }
        }

    }
}
