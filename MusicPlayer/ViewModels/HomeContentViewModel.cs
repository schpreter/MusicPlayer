﻿using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Data;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModels
{
    public partial class HomeContentViewModel : ViewModelBase
    {
        [ObservableProperty]
        private SharedProperties properties;
        public HomeContentViewModel() { }
        public HomeContentViewModel(SharedProperties props)
        {
            properties = props;
        }


    }
}
