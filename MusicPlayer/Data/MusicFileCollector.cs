using M3U8Parser;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;


namespace MusicPlayer.Data
{
    public static class MusicFileCollector
    {
        private const string M3U_HEADER = "#EXTM3U";


        public static ObservableCollection<SongListItem> CollectFilesFromFolder(string folderPath)
        {
            ObservableCollection<SongListItem> returnList = new ObservableCollection<SongListItem>();
            string[] allowedExtensions = new[] { ".flac", ".mp3", ".ogg" };
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
            string configPath = Path.Combine(folderPath, "config.m3u");

            if (songs.Count != 0 && Path.Exists(configPath))
            {

                var masterPlaylist = MasterPlaylist.LoadFromFile(configPath);
                //Using string builder, assuming large collection -> more efficent than string +=
                //TODO: Writing to M3U file, move to different method
                //StringBuilder fileContent = new StringBuilder(M3U_HEADER);
                //foreach (SongListItem item in songs)
                //{
                //    var data = item.SongMetaData;
                //    fileContent.AppendLine($"#EXTINF:{data.Duration.TotalSeconds},{data.Artists_conc} - {data.Title}");
                //    fileContent.AppendLine($"#EXTALB:{data.Album}");
                //    foreach(var genre in data.Genres)
                //    {
                //        fileContent.AppendLine($"#EXTGENRE:{genre}");
                //    }
                //    fileContent.AppendLine(Path.GetFileName(data.FilePath));
                //}
            }
        }
    }
}
