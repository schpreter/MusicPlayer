using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using MusicPlayer.Models;
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
        private enum SkipDirection
        {
            Forward,
            Backward
        }
        private bool IsPaused = true;
        private LibVLC LibVlc = new LibVLC();
        public MediaPlayer MediaPlayer { get; }
        private SharedProperties Properties;
        private string CurrentSongPath = "";

        public MusicNavigationViewModel(SharedProperties props)
        {
            MediaPlayer = new MediaPlayer(LibVlc);
            Properties = props;
        }

        public void ShuffleClicked()
        {
            throw new NotImplementedException();
        }
        public void SkipBackClicked()
        {
            SkipSong(SkipDirection.Backward);
        }
        public void SkipForwardClicked()
        {
            SkipSong(SkipDirection.Forward);
        }
        public void PausePlayClicked()
        {
            if (Properties.SelectedSongListItem != null)
            {
                Properties.CurrentPlayingSong = Properties.SelectedSongListItem;
            }
            PlaySong();

        }
        public void RepeatClicked()
        {
            throw new NotImplementedException();
        }
        private void PlaySong()
        {
            var song = Properties.SelectedSongListItem;
            if (song != null)
            {
                bool newSongSelected = CurrentSongPath != song.SongMetaData.FilePath;
                if (newSongSelected)
                {
                    CurrentSongPath = song.SongMetaData.FilePath;
                }

                using Media? media = new Media(LibVlc, CurrentSongPath);
                if (newSongSelected) { MediaPlayer.Play(media); }
                else
                {
                    MediaPlayer.Pause();
                }
                IsPaused = !IsPaused;
            }
        }
        private void SkipSong(SkipDirection direction)
        {
            int length = Properties.MusicFiles.Count;
            switch (direction)
            {
                case SkipDirection.Forward:
                    {
                        if (Properties.SelectedSongIndex != length - 1) ++Properties.SelectedSongIndex;
                        else Properties.SelectedSongIndex = 0;
                        break;
                    }
                case SkipDirection.Backward:
                    {
                        if (Properties.SelectedSongIndex != 0) --Properties.SelectedSongIndex;
                        else Properties.SelectedSongIndex = length - 1;
                        break;
                    }
            }

            Properties.SelectedSongListItem = Properties.MusicFiles[Properties.SelectedSongIndex];
            Properties.CurrentPlayingSong = Properties.SelectedSongListItem;
            PlaySong();
        }

    }
}
