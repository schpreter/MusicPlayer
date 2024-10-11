using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using MusicPlayer.Models;
using MusicPlayer.Shared;

namespace MusicPlayer.ViewModels
{
    public partial class MusicNavigationViewModel : ViewModelBase
    {
        private enum SkipDirection
        {
            Forward,
            Backward
        }
        [ObservableProperty]
        public bool isPaused = true;
        private LibVLC LibVlc = new LibVLC();
        public MediaPlayer MediaPlayer { get; }

        public MusicNavigationViewModel(SharedProperties props)
        {
            MediaPlayer = new MediaPlayer(LibVlc);
            Properties = props;
        }

        public void ShuffleClicked()
        {

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
            PlaySong();

        }
        public void RepeatClicked()
        {
        }
        private void PlaySong()
        {
            SongItem song = Properties.SelectedSongListItem;
            if (song != null)
            {
                bool newSongSelected = Properties.CurrentPlayingSong != Properties.SelectedSongListItem;
                if (newSongSelected)
                {
                    Properties.CurrentPlayingSong = Properties.SelectedSongListItem;
                }

                using Media media = new Media(LibVlc, Properties.CurrentPlayingSong.FilePath);
                if (newSongSelected)
                {
                    MediaPlayer.Play(media);
                }
                else
                {
                    MediaPlayer.Pause();
                }
                IsPaused = MediaPlayer.IsPlaying;

            }
            else if (MediaPlayer.IsPlaying) MediaPlayer.Pause();

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
