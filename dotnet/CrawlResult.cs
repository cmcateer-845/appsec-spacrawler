using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpaCrawler
{
    internal class CrawlResult
    {
        public Uri SeedUrl { get; }
        public DirectoryInfo OutputFolder { get; private set; }
        public Dictionary<string, CrawlPage> CrawledPages { get; }

        public void WriteResults()
        {
            string crawlSummary = string.Format(
                "Crawl Summary\n" +
                "-------------\n" +
                "Seed Url: {0}\n" +
                "Number Crawled Pages {1}\n",
                SeedUrl.ToString(),
                CrawledPages.Count);

            uint linkNumber = 1;
            foreach(var page in CrawledPages)
            {
                crawlSummary = crawlSummary + 
                    string.Format("Crawled Link #{0}: Url={1}, Details={2}\n",
                    linkNumber,
                    page.Key,
                    page.Value.Guid.ToString().Replace("-", ""));
                linkNumber++;
            }

            File.WriteAllText(Path.Join(OutputFolder.FullName, "Summary.txt"), crawlSummary);
        }
        private void GenerateResultsDirectory()
        {
            Directory.CreateDirectory(@"results");
            DirectoryInfo outputBaseDirectory = new DirectoryInfo(@"results");
            OutputFolder = outputBaseDirectory
                .CreateSubdirectory(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
        }
        public CrawlResult(string seedUrl)
        {
            SeedUrl = new Uri(seedUrl);
            CrawledPages = new Dictionary<string, CrawlPage>();
            GenerateResultsDirectory();
        }
    }
}
