using Xunit;
using MusicPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Headless.XUnit;
using TagLib;
using Moq;
using MusicPlayer.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;

namespace MusicPlayer.ViewModels.Tests
{
    public class UnifiedCategoryViewModelTest
    {
        //Abstract methods will e tested in implementation classes
        //[Fact]
        //public void ShowSongsInCategoryTest()
        //{

        //}

        //[Fact]
        //public void AddSelectedSongsTest()
        //{

        //}

        //[Fact]
        //public void RemoveSelectedSongsTest()
        //{

        //}

        //[Fact]
        //public void RemoveSongTest()
        //{

        //}

        [Fact]
        public void ShowHomeTest()
        {

            Mock<UnifiedCategoryViewModel> vmMock = new Mock<UnifiedCategoryViewModel>();

            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                new SongItem(){IsSelected = true},
                new SongItem(){IsSelected = false},
                new SongItem(){IsSelected = true},
                new SongItem(){IsSelected = false},
            };
            vmMock.Object.Properties = new Shared.SharedProperties();
            vmMock.Object.Properties.MusicFiles = mockSongs;

            vmMock.Object.ShowHome();

            //Bool variables
            Assert.False(vmMock.Object.ShowSongSelectionList);
            Assert.False(vmMock.Object.ShowSongs);
            Assert.True(vmMock.Object.ShowCategoryHome);

            //Properties changed with private methods
            Assert.All(vmMock.Object.Properties.MusicFiles, x => Assert.False(x.IsSelected)); 
            Assert.Empty(vmMock.Object.SongsByCategory);
        }

        [Fact]
        public void ShowSelectionTest()
        {
            Mock<UnifiedCategoryViewModel> vmMock = new Mock<UnifiedCategoryViewModel>();

            vmMock.Object.ShowSelection("Add");
            Assert.True(vmMock.Object.SelectionIsAdd);

            vmMock.Object.ShowSelection(string.Empty);
            Assert.False(vmMock.Object.SelectionIsAdd);

            vmMock.Object.ShowSelection("Something\n\testtesttest");
            Assert.False(vmMock.Object.SelectionIsAdd);


            Assert.True(vmMock.Object.ShowSongSelectionList);
            Assert.False(vmMock.Object.ShowSongs);
            Assert.False(vmMock.Object.ShowCategoryHome);
 

        }

        [Fact]
        public void AddNewCategoryTest()
        {
            Mock<UnifiedCategoryViewModel> vmMock = new Mock<UnifiedCategoryViewModel>();
            vmMock.Object.AddNewCategory();
            Assert.Null(vmMock.Object.SelectedCategory);
        }
    }
}