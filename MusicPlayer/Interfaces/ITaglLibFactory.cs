using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Interfaces
{
    public interface ITaglLibFactory
    {
        public TagLib.File Create(string path);
    }
}
