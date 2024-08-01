using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels
{
    public partial class MusicNavigationViewModel : ViewModelBase
    {

        private bool IsStarted = true;
        private LibVLC LibVlc = new LibVLC();
        private MediaPlayer MediaPlayer { get; }

        public MusicNavigationViewModel()
        {
            MediaPlayer = new MediaPlayer(LibVlc);
        }

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

            using Media? media = new Media(LibVlc, "C:\\Users\\HV387FL\\School\\Szakdoga\\Musics\\steins-gate-anime-complete-soundtrack\\Disc 1\\10. GATE OF STEINER -piano-.flac");
            if (IsStarted) { MediaPlayer.Play(media); IsStarted = false; }
            else MediaPlayer.Pause();
        }
        public void RepeatClicked()
        {
            throw new NotImplementedException();
        }

    }
}
