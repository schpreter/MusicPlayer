using DialogHostAvalonia;

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
