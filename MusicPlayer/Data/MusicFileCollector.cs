﻿using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using TagLib.Id3v2;


namespace MusicPlayer.Data
{
    public static class MusicFileCollector
    {

        public static ObservableCollection<SongListItem> CollectFilesFromFolder(string folderPath)
        {
            ObservableCollection<SongListItem> returnList = new ObservableCollection<SongListItem>();
            string[] allowedExtensions = [".ogg",".mp3",".flac"];
            var listOfFilesInFolder = Directory.GetFiles(folderPath).Where(fil => allowedExtensions.Any(fil.ToLower().EndsWith));

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
                    PlayLists = ParseData(tagLibFile),
                    IsSelected = false

                };
                returnList.Add(songItem);

            }
            return returnList;
        }

        private static List<string> ParseData(TagLib.File tagLibfile)
        {

            switch (tagLibfile.MimeType)
            {
                case "taglib/mp3":
                    {
                        TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tagLibfile.GetTag(TagLib.TagTypes.Id3v2, true);
                        //Writing MP3
                        //PrivateFrame pFrame = PrivateFrame.Get(tag, "Playlists", true);
                        //pFrame.PrivateData = System.Text.Encoding.Unicode.GetBytes("Test MP3 PS");


                        //Reading MP3
                        PrivateFrame pFrame = PrivateFrame.Get(tag, "Playlists", true);


                        //Delimiter will be ; and if the user decides to put that in the playlist name
                        //During storage it will be escaped with \\
                        if(pFrame.PrivateData == null)
                            return new List<string>();
                        string data = Encoding.Unicode.GetString(pFrame.PrivateData.Data);

                        List<string> list = Regex.Split(data, @"(?<!\\);").ToList();
                        //During write EVERY ; in the playlist name needs to be replaced with \;
                        //So theoretically if the user adds \; it will become \\; which hopefully wont turn into an esc character
                        list.ForEach(x => x.Replace(@"\;", ";"));
                        return list;

                    }
                default:
                    {
                        var tag = (TagLib.Ogg.XiphComment)tagLibfile.GetTag(TagLib.TagTypes.Xiph, true);

                        //Write Flac and OGG
                        //tag.SetField("Playlists", new string[] { "PS1", "PS2" });

                        //Read Flac and OGG
                        return tag.GetField("Playlists").ToList();
                    }
            }
        }
    }
}
