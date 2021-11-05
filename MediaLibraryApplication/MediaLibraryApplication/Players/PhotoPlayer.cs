﻿using MediaLibraryApplication.Core;
using MediaLibraryApplication.Core.Players;
using MediaLibraryApplication.Core.PlayList;
using System;

namespace MediaLibraryApplication.Players
{
    public class PhotoPlayer : MediaPlayer
    {
        /// <summary>
        /// Method for playing a photo file
        /// </summary>
        /// <param name="file"></param>
        public override void Play(File file)
        {
            //doing something to show a photo file
            Console.WriteLine($"Photo : {file.Name} is showing!");
        }
        /// <summary>
        /// Method for playing a photo playlist
        /// </summary>
        /// <param name="playList"></param>
        public override void Play(IPlayList playList)
        {
            //doing something to show a photo playlist
            Console.WriteLine($"Photo play list : {playList.Name} is showing!");
        }
    }
}
