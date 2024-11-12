using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibVLCSharp.Shared;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input;

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

        [ObservableProperty]
        public string currentTimeStamp = string.Empty;

        private bool IsSliderDragging { get; set; } = false;
        //[ObservableProperty]
        //public long currentTimeMs = 0;

        private long currentTimeMs = 0;
        public long CurrentTimeMs
        {
            get => currentTimeMs;
            set
            {
                if (IsUserSliderChange && MediaPlayer.IsPlaying)
                {
                    MediaPlayer.Time = value;
                    IsUserSliderChange = false;
                }
                SetProperty(ref currentTimeMs, value);

            }
        }

        private bool IsUserSliderChange { get; set; } = false;

        private LibVLC LibVlc = new LibVLC();

        private MediaPlayer MediaPlayer { get; }

        public MusicNavigationViewModel(SharedProperties props)
        {
            MediaPlayer = new MediaPlayer(LibVlc);
            Properties = props;

            //Registering listeners for the different media player events
            MediaPlayer.EndReached += MediaPlayer_EndReached;
            MediaPlayer.Paused += MediaPlayer_Paused;
            MediaPlayer.Playing += MediaPlayer_Playing;
            MediaPlayer.TimeChanged += MediaPlayer_TimeChanged;


        }

        private void MediaPlayer_EndReached(object sender, System.EventArgs e)
        {
            //Calling back to LibVLC sharp from an event may freeze the app
            //As described by https://github.com/videolan/libvlcsharp/blob/3.x/docs/best_practices.md
            ThreadPool.QueueUserWorkItem(_ => SkipForwardClicked());
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
            if (Properties.SelectedSong != null)
            {

                //If new is selected we switch the playback to that one
                if (IsNewSongSelected())
                {

                    using Media media = new Media(LibVlc, Properties.SelectedSong.FilePath);
                    if (MediaPlayer.Play(media))
                    {
                        MediaPlayer.Time = CurrentTimeMs;

                    }

                    //Stroe the SelectedSongPath for song switches
                    Properties.PreviousSongPath = Properties.SelectedSong.FilePath;

                }
                //Otherwise we just toggle the pause state
                else
                {
                    MediaPlayer.Pause();
                }
                //MediaPlayer.Paused += MediaPlayer_Paused;
                //MediaPlayer.Playing += MediaPlayer_Playing;
                //MediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
                //MediaPlayer.E

            }
        }

        private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            IsUserSliderChange = false;
            if (!IsSliderDragging)
            {
                CurrentTimeMs = e.Time;
                CurrentTimeStamp = TimeSpan.FromMilliseconds(CurrentTimeMs).ToString(@"mm\:ss");
            }

        }

        public void SliderUserChanged(long valueInMs)
        {
            IsUserSliderChange = true;
            IsSliderDragging = false;

            CurrentTimeMs = valueInMs;
            CurrentTimeStamp = TimeSpan.FromMilliseconds(CurrentTimeMs).ToString(@"mm\:ss");
        }

        private void MediaPlayer_Playing(object sender, EventArgs e)
        {
            IsPaused = false;
        }

        /*
         * Event Listener, in case the paused event fires it sets the paused property to true
         */
        private void MediaPlayer_Paused(object sender, System.EventArgs e)
        {
            IsPaused = true;
        }

        /*
         * If the PreviousSong (loaded into the media player that is)
         * Doesn't equal the one we selected, it means a song switching happened
         */
        private bool IsNewSongSelected()
        {
            return Properties.PreviousSongPath != Properties.SelectedSong.FilePath;

        }
        private void SkipSong(SkipDirection direction)
        {
            //TODO: Skipping should be working on either all songs or the songs in a given category
            IEnumerable<SongItem> SongsToPlay;
            if (Properties.SongsByCategory.Count == 0)
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

            Properties.SelectedSong = SongsToPlay.ElementAtOrDefault(Properties.SelectedSongIndex);
            PlaySong();
        }

        public void SliderDragging(long valueInMs)
        {
            IsSliderDragging = true;
            CurrentTimeStamp = TimeSpan.FromMilliseconds(valueInMs).ToString(@"mm\:ss");
        }
    }
}
