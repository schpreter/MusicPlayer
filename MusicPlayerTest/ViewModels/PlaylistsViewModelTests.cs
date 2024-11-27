using Xunit;
using MusicPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MusicPlayer.Shared;
using MusicPlayer.Models;
using System.Collections.ObjectModel;

namespace MusicPlayer.ViewModels.Tests
{
    public class PlaylistsViewModelTests
    {
        private readonly Mock<SharedProperties> _properties = new Mock<SharedProperties>();
        private readonly Mock<NewCategoryInputViewModel> _newCategoryInputViewModel = new Mock<NewCategoryInputViewModel>();

        [Fact()]
        public void PlaylistsViewModelTest()
        {
            PlaylistsViewModel vm = new PlaylistsViewModel(_properties.Object, _newCategoryInputViewModel.Object);

            Assert.NotNull(vm);
            Assert.Empty(vm.ItemCollection);
            Assert.Empty(vm.SongsByCategory);
            Assert.Equal(vm.Properties, _properties.Object);
            Assert.Equal(vm.NewCategoryInputViewModel, _newCategoryInputViewModel.Object);
        }

        [Fact()]
        public void RefreshContentTest()
        {
            Mock<PlaylistsViewModel> vmMock = new Mock<PlaylistsViewModel>(_properties.Object, _newCategoryInputViewModel.Object);

            List<string> list1 = new List<string>() { "List1", "List2", "List3" };
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>() { "List1" };

            SongItem item1 = new SongItem() { PlayLists = list1 };
            SongItem item2 = new SongItem() { PlayLists = list2 };
            SongItem item3 = new SongItem() { PlayLists = list3 };

            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                item1,
                item2,
                item3
            };

            vmMock.Object.Properties.MusicFiles = mockSongs;

            vmMock.CallBase = true; // This tells Moq to call the real methods on the object


            vmMock.Object.RefreshContent();

            Assert.Collection<UnifiedDisplayItem>(vmMock.Object.ItemCollection
                , item => Assert.Equal("List1", item.Name)
                , item => Assert.Equal("List2", item.Name)
                , item => Assert.Equal("List3", item.Name));
        }

        [Fact()]
        public void ShowSongsInCategoryTest()
        {
            Mock<PlaylistsViewModel> vmMock = new Mock<PlaylistsViewModel>(_properties.Object, _newCategoryInputViewModel.Object);

            List<string> list1 = new List<string>() { "List1", "List2", "List3" };
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>() { "List1" };

            SongItem item1 = new SongItem() { PlayLists = list1 };
            SongItem item2 = new SongItem() { PlayLists = list2 };
            SongItem item3 = new SongItem() { PlayLists = list3 };

            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                item1,
                item2,
                item3
            };

            vmMock.Object.Properties.MusicFiles = mockSongs;

            vmMock.CallBase = true;

            vmMock.Object.ShowSongsInCategory("List1");
            Assert.Collection<SongItem>(vmMock.Object.SongsByCategory
                , item => Assert.Equivalent(item1, item)
                , item => Assert.Equivalent(item3, item)
                );

            vmMock.Object.ShowSongsInCategory("List2");
            Assert.Collection<SongItem>(vmMock.Object.SongsByCategory
                , item => Assert.Equivalent(item1, item)
                );

            vmMock.Object.ShowSongsInCategory("");
            Assert.Empty(vmMock.Object.SongsByCategory);

            vmMock.Object.ShowSongsInCategory(null);
            Assert.Empty(vmMock.Object.SongsByCategory);

            vmMock.Object.ShowSongsInCategory("List4");
            Assert.Empty(vmMock.Object.SongsByCategory);

            Assert.True(vmMock.Object.ShowSongs);
            Assert.False(vmMock.Object.ShowCategoryHome);
        }

        [Fact()]
        public void AddSelectedSongsTest()
        {
            Mock<PlaylistsViewModel> vmMock = new Mock<PlaylistsViewModel>(_properties.Object, _newCategoryInputViewModel.Object);

            List<string> list1 = new List<string>() { "List1", "List2", "List3" };
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>() { "List1" };

            SongItem item1 = new SongItem() { PlayLists = list1, IsSelected = true };
            SongItem item2 = new SongItem() { PlayLists = list2, IsSelected = true };
            SongItem item3 = new SongItem() { PlayLists = list3 };

            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                item1,
                item2,
                item3
            };

            vmMock.Object.Properties.MusicFiles = mockSongs;

            vmMock.CallBase = true;
            vmMock.Object.SelectedCategory = "List4";
            //vmMock.Object.SelectedCategory = null;
            vmMock.Object.AddSelectedSongs();
            vmMock.Verify(p => p.ModifySelectedSongs(false), Times.Once());
            vmMock.Verify(p => p.RefreshContent(), Times.Once());
            vmMock.Verify(p => p.ShowSongsInCategory("List4"), Times.Once());



            Assert.Equal(new List<string>() { "List1", "List2", "List3", "List4" }, item1.PlayLists);
            Assert.Equal(new List<string>() { "List4" }, item2.PlayLists);

            Assert.All(mockSongs, song => Assert.False(song.IsSelected));
        }

        [Fact()]
        public void RemoveSingleSongTest()
        {
            Mock<PlaylistsViewModel> vmMock = new Mock<PlaylistsViewModel>(_properties.Object, _newCategoryInputViewModel.Object);

            List<string> list1 = new List<string>() { "List1", "List2", "List3" };
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>() { "List1" };

            SongItem item1 = new SongItem() { PlayLists = list1 };
            SongItem item2 = new SongItem() { PlayLists = list2 };
            SongItem item3 = new SongItem() { PlayLists = list3 };

            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                item1,
                item2,
                item3
            };

            vmMock.Object.Properties.MusicFiles = mockSongs;


            vmMock.CallBase = true;

            vmMock.Object.SelectedCategory = "List1";
            vmMock.Object.RemoveSingleSong(item1);

            Assert.Collection(item1.PlayLists, item => Assert.Equal("List2", item),
                item => Assert.Equal("List3", item));

            //Does nothing
            vmMock.Object.SelectedCategory = string.Empty;
            vmMock.Object.RemoveSingleSong(item2);

            vmMock.Object.SelectedCategory = null;
            vmMock.Object.RemoveSingleSong(item1);

            vmMock.Object.SelectedCategory = "List1234";
            vmMock.Object.RemoveSingleSong(item1);

            vmMock.Verify(p => p.ShowSongsInCategory("List1"), Times.Once());
            vmMock.Verify(p => p.ShowSongsInCategory(string.Empty), Times.Never());
            vmMock.Verify(p => p.ShowSongsInCategory(null), Times.Never());
            vmMock.Verify(p => p.ShowSongsInCategory("List1234"), Times.Never());
        }
    }
}