using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaCrawler
{
    public class Crawler
    {
        private const string _collectAllAnchorsDeepScriptJs =
            @"(sameOrigin = true) => {
                const allElements = [];

                const findAllElements = function(nodes) {
                  for (let i = 0, el; el = nodes[i]; ++i) {
                    allElements.push(el);
                    if (el.shadowRoot) {
                      findAllElements(el.shadowRoot.querySelectorAll('*'));
                    }
                  }
                };

                findAllElements(document.querySelectorAll('*'));

                const filtered = allElements
                  .filter(el => el.localName === 'a' && el.href)
                  .filter(el => el.href !== location.href)
                  .filter(el => {
                    if (sameOrigin) {
                      return new URL(location).origin === new URL(el.href).origin;
                    }
                    return true;
                  })
                  .map(a => a.href);

                return Array.from(new Set(filtered));
            }
            ";
        private Browser _browser { get; set; }
        private Dictionary<string, CrawlPage> _crawledPages { get; set; }
        private CrawlSettings _settings { get; set; }
        private CrawlResult _result { get; set; }
        private async Task CrawlPageAsync(CrawlPage pageToCrawl, uint depth = 0)
        {
            if (depth > _settings.MaxDepth)
                return;

            // If we already crawled the url, we know its children.
            CrawlPage crawledPage;
            if(_crawledPages.TryGetValue(pageToCrawl.Url.ToString(), out crawledPage))
            {
                Console.WriteLine(string.Format("Resuing route: {0}", pageToCrawl.Url));

                pageToCrawl.Title = crawledPage.Title;
                pageToCrawl.ScreenShot = crawledPage.ScreenShot;
                pageToCrawl.ChildPages = crawledPage.ChildPages;

                foreach(var childPage in pageToCrawl.ChildPages)
                {
                    if(_crawledPages.TryGetValue(childPage.Url.ToString(), out crawledPage))
                    {
                        childPage.Title = crawledPage.Title;
                        childPage.ScreenShot = crawledPage.ScreenShot;
                    }
                }
                return;
            }
            else
            {
                Console.WriteLine(string.Format("Loading page: {0}", pageToCrawl.Url));

                Page newPage = await _browser.NewPageAsync();
                await newPage.GoToAsync(pageToCrawl.Url.ToString(), WaitUntilNavigation.Networkidle2);

                var anchorsJsToken = await newPage.EvaluateFunctionAsync(_collectAllAnchorsDeepScriptJs, true);
                var anchors = anchorsJsToken.ToObject<HashSet<string>>();
                anchors = anchors.Where(a => a != _settings.SeedUrl.ToString()).ToHashSet();

                var title = await newPage.EvaluateExpressionAsync("document.title");
                pageToCrawl.Title = title.ToString();

                foreach(var anchor in anchors)
                {
                    pageToCrawl.AddChild(new CrawlPage(new Uri(anchor)));
                }

                if(_settings.ScreenShots)
                {
                    string imageFileName = string.Format(@"{0}_screenshot.png", pageToCrawl.Guid.ToString().Replace("-", ""));
                    Bitmap bitMap = new Bitmap(Image.FromStream(await newPage.ScreenshotStreamAsync()));
                    bitMap.Save(Path.Join(_result.OutputDirectory.FullName, imageFileName));
                    pageToCrawl.ScreenShot = new CrawlScreenShot(bitMap, imageFileName);
                }

                if(_settings.DumpDom)
                {
                    string domFileName = string.Format(@"{0}_dom.html", pageToCrawl.Guid.ToString().Replace("-", ""));
                    var domHtml = await newPage.GetContentAsync();
                    File.WriteAllText(Path.Join(_result.OutputDirectory.FullName, domFileName), domHtml);
                }

                _crawledPages.Add(pageToCrawl.Url.ToString(), pageToCrawl);
                await newPage.CloseAsync();
            }

            // Crawl child pages.
            foreach(var childPage in pageToCrawl.ChildPages)
            {
                await CrawlPageAsync(childPage, depth + 1);
            }
        }
        private async Task PerformCrawlAsync(string seedUrl)
        {
            CrawlPage seed = new CrawlPage(new Uri(_settings.SeedUrl.ToString()));
            await CrawlPageAsync(seed);
        }
        private async Task InitializeCrawl(CrawlSettings settings)
        {
            _settings = settings;

            // Download Chromium browser if not already present.
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            // Create the browser if not already present.
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = _settings.Headless,
                DefaultViewport = new ViewPortOptions { Width = 0, Height = 0 },
                Args = new string[] { "" }
            });

            _crawledPages = new Dictionary<string, CrawlPage>();
            _result = new CrawlResult(new DirectoryInfo(_settings.OutputDirectory));
        }
        private void GenerateResults()
        {
            string crawlSummary = string.Format(
                "Crawl Summary\n" +
                "-------------\n" +
                "Seed Url: {0}\n" +
                "Number Crawled Pages {1}\n",
                _settings.SeedUrl.ToString(),
                _crawledPages.Count());

            uint linkNumber = 1;
            foreach(var page in _crawledPages)
            {
                crawlSummary = crawlSummary + 
                    string.Format("Crawled Link #{0}: Url={1}, Details={2}\n",
                    linkNumber,
                    page.Key,
                    page.Value.Guid.ToString().Replace("-", ""));
                linkNumber++;
            }

            File.WriteAllText(Path.Join(_result.OutputDirectory.FullName, "Summary.txt"), crawlSummary);
        }
        private async Task UninitializeCrawl()
        {
            await _browser.CloseAsync();
        }

        public async Task RunAsync(CrawlSettings settings)
        {
            await InitializeCrawl(settings);
            await PerformCrawlAsync(_settings.SeedUrl.ToString());
            GenerateResults();
            await UninitializeCrawl();
        }
    }
}
