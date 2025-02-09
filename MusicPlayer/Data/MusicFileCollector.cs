﻿using MusicPlayer.Models;
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
    /// <summary>
    /// Static class responsible for parsing files into the application.
    /// </summary>
    public static class MusicFileCollector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath">The source folder where files should be looked for</param>
        /// <returns>A collection of <c>SongItems</c></returns>
        public static ObservableCollection<SongItem> CollectFilesFromFolder(string folderPath)
        {
            List<SongItem> returnList = new List<SongItem>();
            string[] allowedExtensions = [".ogg", ".mp3", ".flac"];
            var listOfFilesInFolder = Directory.GetFiles(folderPath).Where(fil => allowedExtensions.Any(fil.ToLower().EndsWith));

            foreach (string item in listOfFilesInFolder)
            {
                TagLib.File tagLibFile;
                try
                {
                    tagLibFile = TagLib.File.Create(item);

                }
                catch
                {
                    tagLibFile = null;
                }
                if (tagLibFile != null)
                {
                    var pics = tagLibFile.Tag.Pictures;

                    //Parsing data into model
                    SongItem songItem = new SongItem
                    {
                        Album = tagLibFile.Tag.Album ?? string.Empty,
                        Title = tagLibFile.Tag.Title == null ? Path.GetFileName(item).Split('.').First() : tagLibFile.Tag.Title,
                        Artists = tagLibFile.Tag.Performers.ToList(),
                        Genres = tagLibFile.Tag.Genres.ToList(),
                        Year = (int)tagLibFile.Tag.Year,
                        Duration = TimeSpan.FromMilliseconds(tagLibFile.Properties.Duration.TotalMilliseconds),
                        FilePath = tagLibFile.Name,
                        PlayLists = ParseData(tagLibFile),
                        IsSelected = false,
                        Images = tagLibFile.Tag.Pictures.Select(img => img.Data).ToList(),

                    };
                    returnList.Add(songItem);
                }
            }
            return new ObservableCollection<SongItem>(returnList.OrderBy(x => x.Title));
        }

        /// <summary>
        /// Based on the mime type of the file, gathers the previously stored playlist information from it.
        /// </summary>
        /// <param name="tagLibfile">The file instance to be parsed</param>
        /// <returns></returns>
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
