using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels
{
    public partial class MusicNavigationViewModel : ViewModelBase
    {
        [ObservableProperty]
        public bool isPaused = true;
        public void ShuffleClicked()
        {
            throw new NotImplementedException();
        }
        public void SkipBackClicked()
        {
            throw new NotImplementedException();
        }
        public void SkipForwardClicked()
        {
            throw new NotImplementedException();
        }
        public void PausePlayClicked()
        {
            throw new NotImplementedException();
        }
        public void RepeatClicked()
        {
            throw new NotImplementedException();
        }
    }
}
