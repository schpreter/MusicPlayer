using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MusicPlayer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase selectedViewModel;
    [ObservableProperty]
    public string user = "John Doe";
    [ObservableProperty]
    public string greeting;

    public MainViewModel(HomeContentViewModel homeContent) 
    {
        SelectedViewModel = homeContent;
    }

    
}
