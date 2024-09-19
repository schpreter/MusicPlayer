using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;


namespace MusicPlayer.Data
{
    public static class MusicFileCollector
    {


        public static ObservableCollection<SongListItem> CollectFilesFromFolder(string folderPath)
        {
            ObservableCollection<SongListItem> returnList = new ObservableCollection<SongListItem>();
            string[] allowedExtensions = new[] { ".flac" };
            var listOfFilesInFolder = Directory.GetFiles(folderPath).Where(fil => allowedExtensions.Any(fil.ToLower().EndsWith));
            var relativePaths = listOfFilesInFolder.Select(path => Path.GetFileName(path)).ToList();
            foreach (string item in listOfFilesInFolder)
            {
                TagLib.File tagLibFile = TagLib.File.Create(item);
                SongListItem songItem = new SongListItem
                {
                    Album = tagLibFile.Tag.Album,
                    Title = tagLibFile.Tag.Title == null ? Path.GetFileName(item).Split('.').First() : tagLibFile.Tag.Title,
                    Artists = tagLibFile.Tag.Performers.ToList(),
                    Artists_conc = tagLibFile.Tag.JoinedPerformers,
                    Genres = tagLibFile.Tag.Genres.ToList(),
                    Year = (int)tagLibFile.Tag.Year,
                    Duration = TimeSpan.FromSeconds(tagLibFile.Properties.Duration.TotalSeconds),
                    FilePath = tagLibFile.Name,
                    IsSelected = false

                };
                returnList.Add(songItem);

            }
            return returnList;
        }

        public static void ParsePlaylistsAndCategories(List<SongListItem> songs, string folderPath)
        {
            string configPath = Path.Combine(folderPath, "playlists.json");

            if (!File.Exists(configPath)) File.Create(configPath);

            if (songs.Count != 0)
            {
                foreach (SongListItem songItem in songs)
                {
                    TagLib.File tagLibFile = TagLib.File.Create(songItem.FilePath);
                    ParseData(tagLibFile);

                    //// Read
                    //string[] myfields = custom.GetField("MY_TAG");
                    //Console.WriteLine("First MY_TAG entry: {0}", myfields[0]);

                    //// Write
                    //customTag.SetField("MY_TAG", new string[] { "value1", "value2" });
                    //custom.RemoveField("OTHER_FIELD");
                    //rgFile.Save();
                }

            }
        }

        private static void ParseData(TagLib.File tagLibfile)
        {

            switch (tagLibfile.MimeType)
            {
                case "taglib/flac":
                    {
                        tagLibfile.GetTag(TagLib.TagTypes.FlacMetadata,true);
                        break;
                    }
                case "taglib/ogg":
                    {
                        tagLibfile.GetTag(TagLib.TagTypes.Xiph,true);
                        break;
                    }
                default:
                    {
                        tagLibfile.GetTag(TagLib.TagTypes.Id3v2, true);
                        break;
                    }
            }
        }
    }
}
