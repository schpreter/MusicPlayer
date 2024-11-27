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
    public class ArtistsViewModelTests
    {
        private readonly Mock<SharedProperties> _properties = new Mock<SharedProperties>();
        private readonly Mock<NewCategoryInputViewModel> _newCategoryInputViewModel = new Mock<NewCategoryInputViewModel>();

        [Fact()]
        public void ArtistsViewModelTest()
        {
            ArtistsViewModel vm = new ArtistsViewModel(_properties.Object, _newCategoryInputViewModel.Object);

            Assert.NotNull(vm);
            Assert.Empty(vm.ItemCollection);
            Assert.Empty(vm.SongsByCategory);
            Assert.Equal(vm.Properties, _properties.Object);
            Assert.Equal(vm.NewCategoryInputViewModel, _newCategoryInputViewModel.Object);
        }

        [Fact()]
        public void RefreshContentTest()
        {
            Mock<ArtistsViewModel> vmMock = new Mock<ArtistsViewModel>(_properties.Object, _newCategoryInputViewModel.Object);

            List<string> list1 = new List<string>() { "Ren", "Linkin Park", "Ghost" };
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>() { "Ren" };

            SongItem item1 = new SongItem() { Artists = list1 };
            SongItem item2 = new SongItem() { Artists = list2 };
            SongItem item3 = new SongItem() { Artists = list3 };

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
                , item => Assert.Equal("Ghost", item.Name)
                , item => Assert.Equal("Linkin Park", item.Name)
                , item => Assert.Equal("Ren", item.Name));
        }

        [Fact()]
        public void ShowSongsInCategoryTest()
        {
            Mock<ArtistsViewModel> vmMock = new Mock<ArtistsViewModel>(_properties.Object, _newCategoryInputViewModel.Object);

            List<string> list1 = new List<string>() { "Ren", "Linkin Park", "Ghost" };
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>() { "Ren" };

            SongItem item1 = new SongItem() { Artists = list1 };
            SongItem item2 = new SongItem() { Artists = list2 };
            SongItem item3 = new SongItem() { Artists = list3 };

            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                item1,
                item2,
                item3
            };
            vmMock.Object.Properties.MusicFiles = mockSongs;

            vmMock.CallBase = true;

            vmMock.Object.ShowSongsInCategory("Ren");
            Assert.Collection<SongItem>(vmMock.Object.SongsByCategory
                , item => Assert.Equivalent(item1, item)
                , item => Assert.Equivalent(item3, item)
                );

            vmMock.Object.ShowSongsInCategory("Ghost");
            Assert.Collection<SongItem>(vmMock.Object.SongsByCategory
                , item => Assert.Equivalent(item1, item)
                );

            vmMock.Object.ShowSongsInCategory("");
            Assert.Empty(vmMock.Object.SongsByCategory);

            vmMock.Object.ShowSongsInCategory(null);
            Assert.Empty(vmMock.Object.SongsByCategory);

            vmMock.Object.ShowSongsInCategory("Metallica");
            Assert.Empty(vmMock.Object.SongsByCategory);

            Assert.True(vmMock.Object.ShowSongs);
            Assert.False(vmMock.Object.ShowCategoryHome);
        }

        [Fact()]
        public void AddSelectedSongsTest()
        {
            //Most of this will be moved into ModifySelectedSongs in the parent class
            Mock<ArtistsViewModel> vmMock = new Mock<ArtistsViewModel>(_properties.Object, _newCategoryInputViewModel.Object);

            List<string> list1 = new List<string>() { "Rock", "Punk" };
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>() { "Rock" };

            SongItem item1 = new SongItem() { Artists = list1 };
            SongItem item2 = new SongItem() { Artists = list2, IsSelected = true };
            SongItem item3 = new SongItem() { Artists = list3, IsSelected = true };

            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                item1,
                item2,
                item3
            };
            vmMock.Object.Properties.MusicFiles = mockSongs;

            vmMock.CallBase = true;
            vmMock.Object.SelectedCategory = "Lo-fi";
            //vmMock.Object.SelectedCategory = null;
            vmMock.Object.AddSelectedSongs();
            vmMock.Verify(p => p.ModifySelectedSongs(false), Times.Once());
            vmMock.Verify(p => p.RefreshContent(), Times.Once());
            vmMock.Verify(p => p.ShowSongsInCategory("Lo-fi"), Times.Once());



            Assert.Equal(new List<string>() { "Lo-fi" }, item2.Artists);
            Assert.Equal(new List<string>() { "Rock", "Lo-fi" }, item3.Artists);

            Assert.All(mockSongs, song => Assert.False(song.IsSelected));
        }


        [Fact()]
        public void RemoveSingleSongTest()
        {
            Mock<ArtistsViewModel> vmMock = new Mock<ArtistsViewModel>(_properties.Object, _newCategoryInputViewModel.Object);

            List<string> list1 = new List<string>() { "Ren", "Ghost" };
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>() { "Ren" };

            SongItem item1 = new SongItem() { Artists = list1 };
            SongItem item2 = new SongItem() { Artists = list2 };
            SongItem item3 = new SongItem() { Artists = list3 };

            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                item1,
                item2,
                item3
            };
            vmMock.Object.Properties.MusicFiles = mockSongs;


            vmMock.CallBase = true;

            vmMock.Object.SelectedCategory = "Ren";
            vmMock.Object.RemoveSingleSong(item1);

            Assert.Collection(item1.Artists, item => Assert.Equal("Ghost", item));

            //Does nothing
            vmMock.Object.SelectedCategory = string.Empty;
            vmMock.Object.RemoveSingleSong(item2);

            vmMock.Object.SelectedCategory = null;
            vmMock.Object.RemoveSingleSong(item1);

            vmMock.Object.SelectedCategory = "Lo-fi";
            vmMock.Object.RemoveSingleSong(item1);

            vmMock.Verify(p => p.ShowSongsInCategory("Ren"), Times.Once());
            vmMock.Verify(p => p.ShowSongsInCategory(string.Empty), Times.Never());
            vmMock.Verify(p => p.ShowSongsInCategory(null), Times.Never());
        }
    }
}