﻿using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace MusicPlayer.Models
{
    public class UnifiedDisplayItem
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public Bitmap ImageMap { get; set; }
        public UnifiedDisplayItem(string name, Bitmap image = null)
        {
            Name = name;
            if (image != null)
            {
                ImageMap = image;
            }

        }
    }
}
