using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models.Config
{
    /// <summary>
    /// Class structure which Newtonsoft can deserialize the <c>appsettings.json</c> into.
    /// </summary>
    public class Config
    {


        public SourceFolder SOURCE_FOLDERS { get; set; }
        public AuthData AUTH_DATA { get; set; }

    }
}
