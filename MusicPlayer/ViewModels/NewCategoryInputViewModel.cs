using CommunityToolkit.Mvvm.ComponentModel;
using DialogHostAvalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
