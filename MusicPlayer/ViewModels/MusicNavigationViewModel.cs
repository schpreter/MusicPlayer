﻿using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

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

        public MediaPlayer MediaPlayer { get; }

        public MusicNavigationViewModel()
        {

        }
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
                    MediaPlayer.Time = CurrentTimeMs;
                    MediaPlayer.Pause();
                }
            }
        }

        private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            IsUserSliderChange = false;
            if (!IsSliderDragging)
            {
                CurrentTimeMs = e.Time;
            }
            CurrentTimeStamp = TimeSpan.FromMilliseconds(CurrentTimeMs).ToString(@"mm\:ss");


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
            ObservableCollection<SongItem> SongsToPlay;
            CurrentTimeMs = 0;
            CurrentTimeStamp = TimeSpan.FromMilliseconds(CurrentTimeMs).ToString(@"mm\:ss");

            if (Properties.SongsByCategory.Count == 0)
            {
                SongsToPlay = Properties.MusicFiles;
            }
            else
            {
                SongsToPlay = Properties.SongsByCategory;
            }

            var index = SongsToPlay.IndexOf(Properties.SelectedSong);

            int length = SongsToPlay.Count();
            switch (direction)
            {
                case SkipDirection.Forward:
                    {
                        if (index != length - 1)
                        {
                            Properties.SelectedSong = SongsToPlay[++index];
                        }
                        else Properties.SelectedSong = SongsToPlay[0];
                        break;
                    }
                case SkipDirection.Backward:
                    {
                        if (index != 0)
                        {
                            Properties.SelectedSong = SongsToPlay[--index];
                        }
                        else Properties.SelectedSong = SongsToPlay[length - 1];
                        break;
                    }
            }

            PlaySong();
        }

        public void SliderDragging(long valueInMs)
        {
            IsSliderDragging = true;
            //CurrentTimeStamp = TimeSpan.FromMilliseconds(valueInMs).ToString(@"mm\:ss");
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

    }
}
