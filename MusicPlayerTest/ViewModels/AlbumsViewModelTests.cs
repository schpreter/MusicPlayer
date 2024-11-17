using Xunit;
using MusicPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MusicPlayer.Shared;
using TagLib;
using MusicPlayer.Models;
using System.Collections.ObjectModel;

namespace MusicPlayer.ViewModels.Tests
{
    public class AlbumsViewModelTests
    {
        private readonly Mock<SharedProperties> _properties = new Mock<SharedProperties>();
        private readonly Mock<NewCategoryInputViewModel> _newCategoryInputViewModel = new Mock<NewCategoryInputViewModel>();

        [Fact()]
        public void AlbumsViewModelTest()
        {
            AlbumsViewModel vm = new AlbumsViewModel(_properties.Object, _newCategoryInputViewModel.Object);

            Assert.NotNull(vm);
            Assert.Empty(vm.ItemCollection);
            Assert.Empty(vm.SongsByCategory);
            Assert.Equal(vm.Properties, _properties.Object);
            Assert.Equal(vm.NewCategoryInputViewModel, _newCategoryInputViewModel.Object);
        }

        [Fact()]
        public void RefreshContentTest()
        {
            Mock<AlbumsViewModel> vmMock = new Mock<AlbumsViewModel>(_properties.Object,_newCategoryInputViewModel.Object);
            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                new SongItem(){Album = "Album1"},
                new SongItem(){Album = "Album1"},
                new SongItem(){Album = "Album2"},
                new SongItem(){Album = "Album3"},
                new SongItem(){Album = ""},
                new SongItem(){Album = string.Empty},
                new SongItem(){Album = null},
            };

            //vmMock.Object.Properties = new Shared.SharedProperties();
            vmMock.Object.Properties.MusicFiles = mockSongs;

            vmMock.CallBase = true; // This tells Moq to call the real methods on the object


            vmMock.Object.RefreshContent();
            
            Assert.Collection<UnifiedDisplayItem>(vmMock.Object.ItemCollection
                ,item => Assert.Equal("Album1", item.Name)
                ,item => Assert.Equal("Album2", item.Name)
                , item => Assert.Equal("Album3", item.Name));


        }

        [Fact()]
        public void ShowSongsInCategoryTest()
        {
            Mock<AlbumsViewModel> vmMock = new Mock<AlbumsViewModel>();
            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                new SongItem(){Album = "Album1"},
                new SongItem(){Album = "Album1"},
                new SongItem(){Album = "Album2"},
                new SongItem(){Album = "Album3"},
                new SongItem(){Album = "Album3"},
                new SongItem(){Album = "Album3"},
                new SongItem(){Album = ""},
                new SongItem(){Album = string.Empty},
                new SongItem(){Album = null},
            };
            vmMock.Object.Properties = new Shared.SharedProperties();
            vmMock.Object.Properties.MusicFiles = mockSongs;

            vmMock.Object.ShowSongsInCategory("Album1");
            Assert.Collection<SongItem>(vmMock.Object.SongsByCategory
                , item => Assert.Equal("Album1", item.Album)
                , item => Assert.Equal("Album1", item.Album)
                );

            vmMock.Object.ShowSongsInCategory("Album2");
            Assert.Collection<SongItem>(vmMock.Object.SongsByCategory
                , item => Assert.Equal("Album2", item.Album)
                );

            vmMock.Object.ShowSongsInCategory("Album3");
            Assert.Collection<SongItem>(vmMock.Object.SongsByCategory
                , item => Assert.Equal("Album3", item.Album)
                , item => Assert.Equal("Album3", item.Album)
                , item => Assert.Equal("Album3", item.Album)
                );

            vmMock.Object.ShowSongsInCategory("");
            Assert.Empty(vmMock.Object.SongsByCategory);

            vmMock.Object.ShowSongsInCategory(null);
            Assert.Empty(vmMock.Object.SongsByCategory);

            vmMock.Object.ShowSongsInCategory("NonExistentAlbum");
            Assert.Empty(vmMock.Object.SongsByCategory);

            Assert.True(vmMock.Object.ShowSongs);
            Assert.False(vmMock.Object.ShowCategoryHome);
        }

        [Fact()]
        public void AddSelectedSongsTest()
        {
            Xunit.Assert.Fail("This test needs an implementation");
        }

        [Fact()]
        public void ToStringTest()
        {
            Xunit.Assert.Fail("This test needs an implementation");
        }

        [Fact()]
        public void RemoveSelectedSongsTest()
        {
            Xunit.Assert.Fail("This test needs an implementation");
        }

        [Fact()]
        public void RemoveSongTest()
        {
            Xunit.Assert.Fail("This test needs an implementation");
        }
    }
}