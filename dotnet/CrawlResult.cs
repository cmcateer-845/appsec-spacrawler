using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpaCrawler
{
    internal class CrawlResult
    {
        private DirectoryInfo _outputDirectory { get; set; }
        private uint _numberCrawledPages { get; set; }
        private HashSet<Page> _crawledPages { get; set; } 
        public DirectoryInfo OutputDirectory { get => _outputDirectory; }
        public uint NumberCrawledPages
        {
            get => _numberCrawledPages;
            set { if (value > 0) { _numberCrawledPages = value; } }
        }
        public HashSet<Page> CrawledPages
        {
            get => _crawledPages;
            set => _crawledPages = value;
        }
        public CrawlResult(string outputBaseDirectoryStr)
        {
            Directory.CreateDirectory(outputBaseDirectoryStr);
            DirectoryInfo outputBaseDirectory = new DirectoryInfo(outputBaseDirectoryStr);
            _outputDirectory = outputBaseDirectory
                .CreateSubdirectory(DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss"));
        }
        public CrawlResult(DirectoryInfo outputBaseDirectory)
        {
            Directory.CreateDirectory(outputBaseDirectory.FullName);
            _outputDirectory = outputBaseDirectory
                .CreateSubdirectory(DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss"));
        }

    }
}
