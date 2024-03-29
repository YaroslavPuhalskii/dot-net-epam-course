﻿using NLog;
using Sales.Core.Abstractions;
using System;
using System.Configuration;
using System.IO;

namespace Sales.Core
{
    public class FolderWatcher : IDisposable
    {
        private readonly string _startFolder = ConfigurationManager.AppSettings["forManager"];

        private readonly string _processingFolder = ConfigurationManager.AppSettings["processing"];

        private readonly string _processedFolder = ConfigurationManager.AppSettings["processed"];

        private readonly string _errorsFolder = ConfigurationManager.AppSettings["errors"];

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private FileSystemWatcher _watcher;

        private bool disposedValue;

        private readonly IProcessManager _processManager;

        public FolderWatcher(IProcessManager processManager)
        {
            _processManager = processManager;
        }

        public void Start()
        {
            logger.Info("Start dispatcher!");
            Init();
        }

        public void Stop()
        {
            logger.Info("Stop dispatcher!");
            Dispose();
        }

        private void Init()
        {
            _watcher = new FileSystemWatcher(_startFolder)
            {
                NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
            };

            _watcher.Created += OnCreated;

            _watcher.Filter = "*.csv";
            _watcher.EnableRaisingEvents = true;

        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            logger.Info($"Create file: {e.FullPath}");
            string path = string.Concat(_processingFolder, e.Name);

            File.Move(e.FullPath, path);
            logger.Info($"{e.Name} move to {path}");

            _processManager.Run(path).ContinueWith(x =>
            {
                if (x.IsFaulted)
                {
                    var errorsPath = string.Concat(_errorsFolder, e.Name);
                    File.Move(path, errorsPath);
                    logger.Info($"{e.Name} move to {errorsPath}");

                }
                else
                {
                    var processedPath = string.Concat(_processedFolder, e.Name);
                    File.Move(path, processedPath);
                    logger.Info($"{e.Name} move to {processedPath}");
                }
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _watcher.Created -= OnCreated;
                    _watcher.EnableRaisingEvents = false;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
