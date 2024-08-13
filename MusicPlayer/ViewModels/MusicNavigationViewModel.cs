using LibVLCSharp.Shared;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using System;

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
            SongListItem song = Properties.SelectedSongListItem;
            if (song != null)
            {
                bool newSongSelected = Properties.CurrentPlayingSong != Properties.SelectedSongListItem;
                if (newSongSelected)
                {
                    Properties.CurrentPlayingSong = Properties.SelectedSongListItem;
                }

                using Media? media = new Media(LibVlc, Properties.CurrentPlayingSong.SongMetaData.FilePath);
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
