using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TagLib.Id3v2;


namespace MusicPlayer.Data
{
    public static class MusicFileCollector
    {

        public static ObservableCollection<SongItem> CollectFilesFromFolder(string folderPath)
        {
            ObservableCollection<SongItem> returnList = new ObservableCollection<SongItem>();
            string[] allowedExtensions = [".ogg", ".mp3", ".flac"];
            var listOfFilesInFolder = Directory.GetFiles(folderPath).Where(fil => allowedExtensions.Any(fil.ToLower().EndsWith));

            foreach (string item in listOfFilesInFolder)
            {
                TagLib.File tagLibFile = TagLib.File.Create(item);
                var pics = tagLibFile.Tag.Pictures;

                SongItem songItem = new SongItem
                {
                    Album = tagLibFile.Tag.Album,
                    Title = tagLibFile.Tag.Title == null ? Path.GetFileName(item).Split('.').First() : tagLibFile.Tag.Title,
                    Artists = tagLibFile.Tag.Performers.ToList(),
                    Artists_conc = tagLibFile.Tag.JoinedPerformers,
                    Genres = tagLibFile.Tag.Genres.ToList(),
                    Year = (int)tagLibFile.Tag.Year,
                    Duration = TimeSpan.FromSeconds(tagLibFile.Properties.Duration.TotalSeconds),
                    FilePath = tagLibFile.Name,
                    PlayLists = ParseData(tagLibFile),
                    IsSelected = false,
                    Images = tagLibFile.Tag.Pictures.Select(img => img.Data).ToList(),

                };
                returnList.Add(songItem);

            }
            return returnList;
        }
        //TODO: Fix folder selection
        //Song skip issue
        private static List<string> ParseData(TagLib.File tagLibfile)
        {

            switch (tagLibfile.MimeType)
            {
                case "taglib/mp3":
                    {
                        TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tagLibfile.GetTag(TagLib.TagTypes.Id3v2, true);
                        PrivateFrame pFrame = PrivateFrame.Get(tag, "Playlists", true);
                        List<string> list = new List<string>();

                        if (pFrame.PrivateData != null)
                        {

                            string data = Encoding.Unicode.GetString(pFrame.PrivateData.Data);

                            if (data != null)
                            {
                                list = Regex.Split(data, @"(?<!\\);").ToList();
                                list.ForEach(x => x.Replace(@"\;", ";"));
                            }
                        }

                        return list;

                    }
                default:
                    {
                        var tag = (TagLib.Ogg.XiphComment)tagLibfile.GetTag(TagLib.TagTypes.Xiph, true);

                        //Read Flac and OGG
                        return tag.GetField("Playlists").ToList();
                    }
            }
        }
    }
}
