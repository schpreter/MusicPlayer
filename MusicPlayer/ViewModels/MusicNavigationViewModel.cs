using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace MusicPlayer.ViewModels
{
    /// <summary>
    /// Class for which the <c>MusicNavigationView</c> view binds to.
    /// </summary>
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

        /// <summary>
        /// Binding command for backward skipping, used by the UI
        /// </summary>
        public void SkipBackClicked()
        {
            SkipSong(SkipDirection.Backward);
        }
        /// <summary>
        /// Binding command for forward skipping, used by the UI
        /// </summary>
        public void SkipForwardClicked()
        {
            SkipSong(SkipDirection.Forward);
        }
        /// <summary>
        /// Binding command for playing and pausing, used by the UI
        /// </summary>
        public void PausePlayClicked()
        {
            PlaySong();

        }

        /// <summary>
        /// Based on the current state of the <c>MediaPlayer</c> starts or pauses playback
        /// </summary>
        private void PlaySong()
        {
            if (Properties.SelectedSong != null)
            {

                //If new is selected switch the playback to that one
                if (IsNewSongSelected())
                {
                    using Media media = new Media(LibVlc, Properties.SelectedSong.FilePath);

                    if (MediaPlayer.Play(media))
                    {
                        MediaPlayer.Time = CurrentTimeMs;

                    }
                    //Store the SelectedSongPath for song switches
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

        /// <summary>
        /// If the PreviousSong (loaded into the media player that is)
        /// Doesn't equal the one selected, it means a song switching happened
        /// </summary>
        /// <returns></returns>
        private bool IsNewSongSelected()
        {
            return Properties.PreviousSongPath != Properties.SelectedSong.FilePath;

        }

        /// <summary>
        /// Skips to the next/previous songs based on the given parameter.
        /// </summary>
        /// <param name="direction"><c>Foward</c> or <c>Backward</c> enum type.</param>
        private void SkipSong(SkipDirection direction)
        {
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
        /// <summary>
        /// Sets the associated <c>bool</c> to <c>true</c>.
        /// </summary>
        /// <param name="valueInMs"></param>
        public void SliderDragging(long valueInMs)
        {
            IsSliderDragging = true;
            CurrentTimeStamp = TimeSpan.FromMilliseconds(valueInMs).ToString(@"mm\:ss");
        }
        /// <summary>
        /// In case the user is the one changing the slider, sets the associated parameters.
        /// </summary>
        /// <param name="valueInMs">The value of the slider, coming from the view.</param>
        public void SliderUserChanged(long valueInMs)
        {
            IsUserSliderChange = true;
            IsSliderDragging = false;

            CurrentTimeMs = valueInMs;
            CurrentTimeStamp = TimeSpan.FromMilliseconds(CurrentTimeMs).ToString(@"mm\:ss");
        }
        #region Event Handlers
        /// <summary>
        /// If the current playback reaches it's end, skips to the next song
        /// </summary>
        /// <param name="sender">The <c>MediaPlayer</c> object.</param>
        /// <param name="e">Event arguments.</param>
        private void MediaPlayer_EndReached(object sender, System.EventArgs e)
        {
            //Calling back to LibVLC sharp from an event may freeze the app
            //As described by https://github.com/videolan/libvlcsharp/blob/3.x/docs/best_practices.md
            ThreadPool.QueueUserWorkItem(_ => SkipForwardClicked());
        }
        /// <summary>
        /// Provides synchronization between UI and data for the slider.
        /// </summary>
        /// <param name="sender">The <c>Slider</c> object.</param>
        /// <param name="e">Event arguments.</param>
        private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            IsUserSliderChange = false;
            if (!IsSliderDragging)
            {
                CurrentTimeMs = e.Time;
            }
            CurrentTimeStamp = TimeSpan.FromMilliseconds(CurrentTimeMs).ToString(@"mm\:ss");


        }
        /// <summary>
        /// Sets the associated <c>IsPaused</c> boolean to <c>false</c>;
        /// </summary>
        /// <param name="sender">The <c>MediaPlayer</c> object.</param>
        /// <param name="e">Event arguments.</param>
        private void MediaPlayer_Playing(object sender, EventArgs e)
        {
            IsPaused = false;
        }

        /// <summary>
        /// Sets the associated <c>IsPaused</c> boolean to <c>true</c>;
        /// </summary>
        /// <param name="sender">The <c>MediaPlayer</c> object.</param>
        /// <param name="e">Event arguments.</param>
        private void MediaPlayer_Paused(object sender, System.EventArgs e)
        {
            IsPaused = true;
        }
        #endregion

    }
}
