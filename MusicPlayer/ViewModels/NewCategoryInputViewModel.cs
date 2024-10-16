using CommunityToolkit.Mvvm.ComponentModel;
using DialogHostAvalonia;

namespace MusicPlayer.ViewModels
{
    public partial class NewCategoryInputViewModel : ViewModelBase
    {

        [ObservableProperty]
        public string newCategoryInput;

        [ObservableProperty]
        public string description;

        [ObservableProperty]
        public string title;

        public NewCategoryInputViewModel()
        {

        }
        public void SubmitNewCategory()
        {
            DialogHost.GetDialogSession("CategoryView")?.Close(NewCategoryInput);

        }


        public void CancelNewCategory()
        {
            DialogHost.GetDialogSession("CategoryView")?.Close(null);
        }

        private void ClearSongSelection()
        {
            foreach (var item in Properties.MusicFiles)
            {
                item.IsSelected = false;
            }
        }
    }
}
