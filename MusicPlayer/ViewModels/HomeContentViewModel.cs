using MusicPlayer.Shared;

namespace MusicPlayer.ViewModels
{
    public partial class HomeContentViewModel : ViewModelBase
    {

        public HomeContentViewModel() { }
        public HomeContentViewModel(SharedProperties props)
        {
            this.Properties = props;
        }

        public override string ToString()
        {
            return "Home";
        }

    }
}
