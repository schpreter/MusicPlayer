using CommunityToolkit.Mvvm.ComponentModel;
using DialogHostAvalonia;

namespace MusicPlayer.ViewModels
{
    /// <summary>
    /// Class which the <c>NewCategoryInput</c> view binds to.
    /// </summary>
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

    }
}
