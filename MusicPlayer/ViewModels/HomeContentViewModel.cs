using MusicPlayer.Shared;

namespace MusicPlayer.ViewModels
{
    /// <summary>
    /// Class which the <c>HomeContentView</c> binds to.
    /// </summary>
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
