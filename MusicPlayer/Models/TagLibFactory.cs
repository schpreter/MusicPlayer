using MusicPlayer.Interfaces;
using System;
using TagLib;

namespace MusicPlayer.Models
{
    public class TagLibFactory : ITaglLibFactory
    {
        public File Create(string path)
        {
            try
            {
                return TagLib.File.Create(path);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString()); 
                return null;
            }
        }
    }
}
