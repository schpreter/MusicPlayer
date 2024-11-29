using Xunit;
using MusicPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MusicPlayer.Shared;
using LibVLCSharp.Shared;
using MusicPlayer.Models;
using System.Collections.ObjectModel;

namespace MusicPlayer.ViewModels.Tests
{
    public class MusicNavigationViewModelTests
    {
        private readonly Mock<SharedProperties> _properties = new Mock<SharedProperties>();

        [Fact()]
        public void MusicNavigationViewModelTest()
        {
            MusicNavigationViewModel vm = new MusicNavigationViewModel(_properties.Object);

            Assert.NotNull(vm);
            Assert.Equal(vm.Properties, _properties.Object);
            Assert.NotNull(vm.MediaPlayer);
        }

        [Fact()]
        public void SkipBackClickedTest()
        {
            Mock<MusicNavigationViewModel> vmMock = new Mock<MusicNavigationViewModel>(_properties.Object);

            SongItem song1 = new SongItem() { Title = "Title1", FilePath = "test.mp3" };
            SongItem song2 = new SongItem() { Title = "Title2" };
            SongItem song3 = new SongItem() { Title = "Title3", FilePath= "test2.mp3" };


            vmMock.Object.Properties.SelectedSong = song2;
            vmMock.Object.Properties.SongsByCategory = new ObservableCollection<SongItem>()
            {
                song1, song2, song3
            };

            //vmMock.CallBase = true;

            vmMock.Object.SkipBackClicked();
            Assert.Equal(song1, vmMock.Object.Properties.SelectedSong);
            //Assert.False(vmMock.Object.IsPaused);

            vmMock.Object.SkipBackClicked();
            Assert.Equal(song3, vmMock.Object.Properties.SelectedSong);
            //Assert.False(vmMock.Object.IsPaused);

        }

        [Fact()]
        public void SkipForwardClickedTest()
        {
            Mock<MusicNavigationViewModel> vmMock = new Mock<MusicNavigationViewModel>(_properties.Object);

            SongItem song1 = new SongItem() { Title = "Title1", FilePath = "test.mp3" };
            SongItem song2 = new SongItem() { Title = "Title2" };
            SongItem song3 = new SongItem() { Title = "Title3", FilePath = "test2.mp3" };

            vmMock.Object.Properties.SelectedSong = song2;
            vmMock.Object.Properties.SongsByCategory = new ObservableCollection<SongItem>()
            {
                song1, song2, song3
            };

            vmMock.Object.SkipForwardClicked();
            Assert.Equal(song3, vmMock.Object.Properties.SelectedSong);
            //Assert.False(vmMock.Object.IsPaused);

            vmMock.Object.SkipForwardClicked();
            Assert.Equal(song1, vmMock.Object.Properties.SelectedSong);
            //Assert.False(vmMock.Object.IsPaused);
        }

        [Fact()]
        public void PausePlayClickedTest()
        {
            Mock<MusicNavigationViewModel> vmMock = new Mock<MusicNavigationViewModel>(_properties.Object);

            SongItem item1 = new SongItem() { FilePath = "Dummypath"};


            vmMock.CallBase = true;

            vmMock.Object.Properties.SelectedSong = null;

            vmMock.Object.PausePlayClicked();

            Assert.Null(vmMock.Object.Properties.SelectedSong);
            Assert.Null(vmMock.Object.Properties.PlayingSong);


            vmMock.Object.Properties.SelectedSong = item1;
            vmMock.Object.PausePlayClicked();

            Assert.Equal(item1,vmMock.Object.Properties.SelectedSong);
            Assert.Equal(item1.FilePath,vmMock.Object.Properties.PlayingSong.FilePath);

        }

        [Fact()]
        public void SliderUserChangedTest()
        {
            Mock<MusicNavigationViewModel> vmMock = new Mock<MusicNavigationViewModel>(_properties.Object);

            int msTest = 1200;
            string msTimesSpanTest = TimeSpan.FromMilliseconds(msTest).ToString(@"mm\:ss");

            vmMock.Object.SliderUserChanged(msTest);

            Assert.Equal(msTest, vmMock.Object.CurrentTimeMs);
            Assert.Equal(msTimesSpanTest, vmMock.Object.CurrentTimeStamp);
        }

        [Fact()]
        public void SliderDraggingTest()
        {
            Mock<MusicNavigationViewModel> vmMock = new Mock<MusicNavigationViewModel>(_properties.Object);

            int msTest1 = 1200;
            string msTimesSpanTest = TimeSpan.FromMilliseconds(msTest1).ToString(@"mm\:ss");

            vmMock.Object.CurrentTimeMs = msTest1;
            vmMock.Object.CurrentTimeStamp = msTimesSpanTest;

            vmMock.CallBase = true;

            vmMock.Object.SliderDragging(msTest1);

            Assert.Equal(msTest1, vmMock.Object.CurrentTimeMs);
            Assert.Equal(msTimesSpanTest, vmMock.Object.CurrentTimeStamp);

            int msTest2 = 1400;
            string msTimesSpanTest2 = TimeSpan.FromMilliseconds(msTest2).ToString(@"mm\:ss");

            Assert.Equal(msTest1, vmMock.Object.CurrentTimeMs);
            Assert.Equal(msTimesSpanTest2, vmMock.Object.CurrentTimeStamp);
        }
    }
}