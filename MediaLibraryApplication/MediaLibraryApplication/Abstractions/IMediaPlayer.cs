﻿using MediaLibraryApplication.Models.DataModel;

namespace MediaLibraryApplication.Abstractions
{
    public interface IMediaPlayer
    {
        void Play(IFile file);
    }
}
