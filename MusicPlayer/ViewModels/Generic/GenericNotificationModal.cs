using DialogHostAvalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels.Generic
{
    public class GenericNotificationModal : ViewModelBase
    {
        public string Title { get; set; }
        public string Message { get; set; }


        public void CloseModal()
        {
            DialogHost.GetDialogSession("GenericModal")?.Close(null);
        }
    }
}
