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
            string[] allowedExtensions = new[] {  ".flac",".mp3",".ogg" };
            var listOfFilesInFolder = Directory.GetFiles(folderPath).Where(fil => allowedExtensions.Any(fil.ToLower().EndsWith));
            foreach (string item in listOfFilesInFolder)
            {
                TagLib.File tagLibFile = TagLib.File.Create(item);
                SongMetaData metaData = new SongMetaData
                {
                    Album = tagLibFile.Tag.Album,
                    Title = tagLibFile.Tag.Title == null ? Path.GetFileName(item).Split('.').First() : tagLibFile.Tag.Title,
                    Artists = tagLibFile.Tag.Performers.ToList(),
                    Artists_conc = tagLibFile.Tag.JoinedPerformers,
                    Genres = tagLibFile.Tag.Genres.ToList(),
                    Year = (int)tagLibFile.Tag.Year,
                    Duration = TimeSpan.FromSeconds(tagLibFile.Properties.Duration.TotalSeconds),
                    FilePath = tagLibFile.Name

                };
                returnList.Add(new SongListItem(metaData));

            }
            return returnList;
        }
    }
}
