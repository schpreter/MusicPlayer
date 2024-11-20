using Moq;
using MusicPlayer.Interfaces;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using System.Collections.ObjectModel;

namespace MusicPlayer.ViewModels.Tests
{
    public class AlbumsViewModelTests
    {
        private readonly Mock<SharedProperties> _properties = new Mock<SharedProperties>();
        private readonly Mock<NewCategoryInputViewModel> _newCategoryInputViewModel = new Mock<NewCategoryInputViewModel>();
        private readonly Mock<ITaglLibFactory> _tagLibFactory = new Mock<ITaglLibFactory>();

        [Fact()]
        public void AlbumsViewModelTest()
        {
            AlbumsViewModel vm = new AlbumsViewModel(_properties.Object, _newCategoryInputViewModel.Object, _tagLibFactory.Object);

            Assert.NotNull(vm);
            Assert.Empty(vm.ItemCollection);
            Assert.Empty(vm.SongsByCategory);
            Assert.Equal(vm.Properties, _properties.Object);
            Assert.Equal(vm.NewCategoryInputViewModel, _newCategoryInputViewModel.Object);
        }

        [Fact()]
        public void RefreshContentTest()
        {
            Mock<AlbumsViewModel> vmMock = new Mock<AlbumsViewModel>(_properties.Object, _newCategoryInputViewModel.Object, _tagLibFactory.Object);
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
                , item => Assert.Equal("Album1", item.Name)
                , item => Assert.Equal("Album2", item.Name)
                , item => Assert.Equal("Album3", item.Name));


        }

        [Fact()]
        public void ShowSongsInCategoryTest()
        {
            Mock<AlbumsViewModel> vmMock = new Mock<AlbumsViewModel>(_properties.Object, _newCategoryInputViewModel.Object, _tagLibFactory.Object);
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
            vmMock.Object.Properties.MusicFiles = mockSongs;

            vmMock.CallBase = true;

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

        ////Could try to test this with Avalonias UI tester later on
        [Fact()]
        public void AddSelectedSongsTest()
        {
            //Most of this will be moved into ModifySelectedSongs in the parent class
            Mock<AlbumsViewModel> vmMock = new Mock<AlbumsViewModel>(_properties.Object, _newCategoryInputViewModel.Object, _tagLibFactory.Object);
            //SongItem item1 = new SongItem() { Album = "Album2", IsSelected = true };
            //SongItem item2 = new SongItem() { Album = "", IsSelected = true };
            //SongItem item3 = new SongItem() { Album = null, IsSelected = true };

            //ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            //{
            //    item1,
            //    item2,
            //    item3,
            //};
            //vmMock.Object.Properties.MusicFiles = mockSongs;


            vmMock.CallBase = true;


            //vmMock.Object.SelectedCategory = null;
            vmMock.Object.AddSelectedSongs();

            //Assert.Equal("Album2", item1.Album);
            //Assert.Equal("", item2.Album);
            //Assert.Null(item3.Album);

            vmMock.Verify(p => p.ModifySelectedSongs(false), Times.Once());
            //Assert.All(mockSongs, song => Assert.True(song.IsSelected));


            //vmMock.Object.SelectedCategory = "Album1";
            //vmMock.Object.AddSelectedSongs();

            //Assert.Equal("Album1", item1.Album);
            //Assert.Equal("Album1", item2.Album);
            //Assert.Equal("Album1", item3.Album);


            //Assert.All(mockSongs, song => Assert.False(song.IsSelected));

        }

        [Fact()]
        public void RemoveSongTest()
        {
            Mock<AlbumsViewModel> vmMock = new Mock<AlbumsViewModel>(_properties.Object, _newCategoryInputViewModel.Object, _tagLibFactory.Object);

            SongItem item1 = new SongItem() { Album = "Album2", IsSelected = true };
            SongItem item2 = new SongItem() { Album = "", IsSelected = true };
            SongItem item3 = new SongItem() { Album = null, IsSelected = true };

            vmMock.CallBase = true;

            vmMock.Object.SelectedCategory = "Album2";
            vmMock.Object.RemoveSingleSong(item1);

            Assert.Empty(item1.Album);

            //Does nothing
            vmMock.Object.SelectedCategory = string.Empty;
            vmMock.Object.RemoveSingleSong(item2);

            vmMock.Object.SelectedCategory = null;
            vmMock.Object.RemoveSingleSong(item1);

            vmMock.Object.SelectedCategory = "NonExistentAlbum";
            vmMock.Object.RemoveSingleSong(item3);

            vmMock.Verify(p => p.ModifySelectedSongs(false), Times.Once());

        }
    }
}