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
using TagLib;

namespace MusicPlayer.ViewModels.Tests
{
    public class GenresViewModelTests
    {
        private readonly Mock<SharedProperties> _properties = new Mock<SharedProperties>();
        private readonly Mock<NewCategoryInputViewModel> _newCategoryInputViewModel = new Mock<NewCategoryInputViewModel>();

        [Fact()]
        public void GenresViewModelTest()
        {
            GenresViewModel vm = new GenresViewModel(_properties.Object, _newCategoryInputViewModel.Object);

            Assert.NotNull(vm);
            Assert.Empty(vm.ItemCollection);
            Assert.Empty(vm.SongsByCategory);
            Assert.Equal(vm.Properties, _properties.Object);
            Assert.Equal(vm.NewCategoryInputViewModel, _newCategoryInputViewModel.Object);
        }

        [Fact()]
        public void RefreshContentTest()
        {
            Mock<GenresViewModel> vmMock = new Mock<GenresViewModel>(_properties.Object, _newCategoryInputViewModel.Object);

            List<string> list1 = new List<string>() { "Rock", "Punk" };
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>() { "Rock" };

            SongItem item1 = new SongItem() { Genres = list1 };
            SongItem item2 = new SongItem() { Genres = list2 };
            SongItem item3 = new SongItem() { Genres = list3 };

            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                item1,
                item2,
                item3
            };

            //vmMock.Object.Properties = new Shared.SharedProperties();
            vmMock.Object.Properties.MusicFiles = mockSongs;

            vmMock.CallBase = true; // This tells Moq to call the real methods on the object


            vmMock.Object.RefreshContent();

            Assert.Collection<UnifiedDisplayItem>(vmMock.Object.ItemCollection
                , item => Assert.Equal("Punk", item.Name)
                , item => Assert.Equal("Rock", item.Name));
        }

        [Fact()]
        public void ShowSongsInCategoryTest()
        {
            Mock<GenresViewModel> vmMock = new Mock<GenresViewModel>(_properties.Object, _newCategoryInputViewModel.Object);

            List<string> list1 = new List<string>() { "Rock", "Punk" };
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>() { "Rock" };

            SongItem item1 = new SongItem() { Genres = list1 };
            SongItem item2 = new SongItem() { Genres = list2 };
            SongItem item3 = new SongItem() { Genres = list3 };

            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                item1,
                item2,
                item3
            };
            vmMock.Object.Properties.MusicFiles = mockSongs;

            vmMock.CallBase = true;

            vmMock.Object.ShowSongsInCategory("Rock");
            Assert.Collection<SongItem>(vmMock.Object.SongsByCategory
                , item => Assert.Equivalent(item1, item)
                , item => Assert.Equivalent(item3, item)
                );

            vmMock.Object.ShowSongsInCategory("Punk");
            Assert.Collection<SongItem>(vmMock.Object.SongsByCategory
                , item => Assert.Equivalent(item1, item)
                );

            vmMock.Object.ShowSongsInCategory("");
            Assert.Empty(vmMock.Object.SongsByCategory);

            vmMock.Object.ShowSongsInCategory(null);
            Assert.Empty(vmMock.Object.SongsByCategory);

            vmMock.Object.ShowSongsInCategory("Lo-fi");
            Assert.Empty(vmMock.Object.SongsByCategory);

            Assert.True(vmMock.Object.ShowSongs);
            Assert.False(vmMock.Object.ShowCategoryHome);
        }

        [Fact()]
        public void AddSelectedSongsTest()
        {
            //Most of this will be moved into ModifySelectedSongs in the parent class
            Mock<GenresViewModel> vmMock = new Mock<GenresViewModel>(_properties.Object, _newCategoryInputViewModel.Object);

            List<string> list1 = new List<string>() { "Rock", "Punk" };
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>() { "Rock" };

            SongItem item1 = new SongItem() { Genres = list1 };
            SongItem item2 = new SongItem() { Genres = list2, IsSelected = true };
            SongItem item3 = new SongItem() { Genres = list3, IsSelected = true };

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



            Assert.Equal(new List<string>() { "Lo-fi"}, item2.Genres);
            Assert.Equal(new List<string>() { "Rock", "Lo-fi"}, item3.Genres);

            Assert.All(mockSongs, song => Assert.False(song.IsSelected));
        }

        [Fact()]
        public void RemoveSingleSongTest()
        {
            Mock<GenresViewModel> vmMock = new Mock<GenresViewModel>(_properties.Object, _newCategoryInputViewModel.Object);

            List<string> list1 = new List<string>() { "Rock", "Punk" };
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>() { "Rock" };

            SongItem item1 = new SongItem() { Genres = list1 };
            SongItem item2 = new SongItem() { Genres = list2 };
            SongItem item3 = new SongItem() { Genres = list3 };

            ObservableCollection<SongItem> mockSongs = new ObservableCollection<SongItem>()
            {
                item1,
                item2,
                item3
            };
            vmMock.Object.Properties.MusicFiles = mockSongs;

            vmMock.CallBase = true;

            vmMock.Object.SelectedCategory = "Rock";
            vmMock.Object.RemoveSingleSong(item1);

            Assert.Collection(item1.Genres, item => Assert.Equal("Punk",item));

            //Does nothing
            vmMock.Object.SelectedCategory = string.Empty;
            vmMock.Object.RemoveSingleSong(item2);

            vmMock.Object.SelectedCategory = null;
            vmMock.Object.RemoveSingleSong(item1);

            vmMock.Object.SelectedCategory = "Lo-fi";
            vmMock.Object.RemoveSingleSong(item1);

            vmMock.Verify(p => p.ShowSongsInCategory("Rock"), Times.Once());
            vmMock.Verify(p => p.ShowSongsInCategory(string.Empty), Times.Never());
            vmMock.Verify(p => p.ShowSongsInCategory(null), Times.Never());
        }
    }
}