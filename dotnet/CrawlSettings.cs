using System;
using System.Collections.Generic;
using System.Text;

namespace SpaCrawler
{
    public class CrawlSettings
    {
        public Uri SeedUrl { get; }
        public uint MaxDepth { get; }
        public bool Headless { get; }
        public bool ScreenShots { get; }
        public bool DumpDom { get; }

        public CrawlSettings(Uri seedUrl,
            uint maxDepth,
            bool headless,
            bool screenShots,
            bool dumpDom)
        {
            SeedUrl = seedUrl;
            MaxDepth = maxDepth;
            Headless = headless;
            ScreenShots = screenShots;
            DumpDom = dumpDom;
        }
    }
}
