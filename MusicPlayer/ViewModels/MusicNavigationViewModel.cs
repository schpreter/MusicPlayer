using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using MusicPlayer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels
{
    public partial class MusicNavigationViewModel : ViewModelBase
    {

        private bool IsPlaying = false;
        private LibVLC LibVlc = new LibVLC();
        private MediaPlayer MediaPlayer { get; }
        private SharedProperties properties;
        private string CurrentSongPlaying = "";

        public MusicNavigationViewModel(SharedProperties props)
        {
            MediaPlayer = new MediaPlayer(LibVlc);
            properties = props;
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
            bool newSongSelected = CurrentSongPlaying != properties.SelectedSongListItem.SongMetaData.FilePath;
            if (newSongSelected)
            {
                CurrentSongPlaying = properties.SelectedSongListItem.SongMetaData.FilePath;
            }

            using Media? media = new Media(LibVlc, CurrentSongPlaying);
            if (!IsPlaying || newSongSelected) { MediaPlayer.Play(media); IsPlaying = true; }
            else MediaPlayer.Pause();


        }
        public void RepeatClicked()
        {
            throw new NotImplementedException();
        }

    }
}
