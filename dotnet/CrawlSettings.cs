using System;
using System.Collections.Generic;
using System.Text;

namespace SpaCrawler
{
    public class CrawlSettings
    {
        private Uri _seedUrl { get; set; }
        private string _outputDirectory { get; set; }
        private string _playbackFile { get; set; }
        private uint _maxDepth { get; set; }
        private bool _headless { get; set; }
        private bool _screenShots { get; set; }
        private bool _dumpDom { get; set; }
        public Uri SeedUrl { get => _seedUrl; }
        public string OutputDirectory { get => _outputDirectory; }
        public string LoginScript { get => _playbackFile; }
        public uint MaxDepth { get => _maxDepth; }
        public bool Headless { get => _headless; }
        public bool ScreenShots { get => _screenShots; }
        public bool DumpDom { get => _dumpDom; }
        public CrawlSettings(Uri seedUrl,
            string outputDirectory,
            string playbackFile,
            uint maxDepth,
            bool headless,
            bool screenShots,
            bool dumpDom)
        {
            _seedUrl = seedUrl;
            _outputDirectory = outputDirectory;
            _playbackFile = playbackFile;
            _maxDepth = maxDepth;
            _headless = headless;
            _screenShots = screenShots;
            _dumpDom = dumpDom;
        }
    }
}
