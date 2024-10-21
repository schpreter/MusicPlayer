using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using System.Collections.Generic;
using System.Linq;
using TagLib.Riff;

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
        private MediaPlayer MediaPlayer { get; }

        public MusicNavigationViewModel(SharedProperties props)
        {
            MediaPlayer = new MediaPlayer(LibVlc);
            Properties = props;
        }

        public void ShuffleClicked()
        {
            //TODO
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
            //TODO
        }
        private void PlaySong()
        {
            //TODO: Refactor, this is ugly code
            if (Properties.SelectedSong != null)
            {
                bool newSongSelected = IsNewSongSelected();
                //Change the song we want to play into the one the user selected
                if (newSongSelected)
                    Properties.CurrentSong = Properties.SelectedSong;

                using Media media = new Media(LibVlc, Properties.CurrentSong.FilePath);
                //If new is selected we switch the playback to that one
                if (newSongSelected)
                {
                    MediaPlayer.Play(media);
                }
                //Otherwise we just toggle the pause state
                else
                {
                    MediaPlayer.Pause();
                }
                IsPaused = !MediaPlayer.IsPlaying;

            }
            //else if (MediaPlayer.IsPlaying) MediaPlayer.Pause();

        }

        private bool IsNewSongSelected()
        {
            return Properties.CurrentSong != Properties.SelectedSong;

        }
        private void SkipSong(SkipDirection direction)
        {
            //TODO: Skipping should be working on either all songs or the songs in a given category
            IEnumerable<SongItem> SongsToPlay;
            if (Properties.SongsByCategory == null || !Properties.SongsByCategory.Any())
            {
                SongsToPlay = Properties.MusicFiles;
            }
            else
            {
                SongsToPlay = Properties.SongsByCategory;
            }
            
            int length = SongsToPlay.Count();
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

            Properties.SelectedSong = Properties.MusicFiles[Properties.SelectedSongIndex];
            Properties.CurrentSong = Properties.SelectedSong;
            PlaySong();
        }

    }
}
