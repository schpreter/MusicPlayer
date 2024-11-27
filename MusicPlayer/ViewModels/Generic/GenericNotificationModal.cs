using DialogHostAvalonia;

namespace MusicPlayer.ViewModels.Generic
{
    /// <summary>
    /// Notification modal binder class, mostly used for error messages.
    /// </summary>
    public class GenericNotificationModal : ViewModelBase
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public GenericNotificationModal(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public void CloseModal()
        {
            DialogHost.GetDialogSession("GenericModal")?.Close(null);
        }
    }
}
