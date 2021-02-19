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
        private Browser browser;
        private CrawlSettings settings;
        private CrawlResult result;

        private async Task InitializeCrawl(CrawlSettings settings)
        {
            this.settings = settings;
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = settings.Headless,
                DefaultViewport = new ViewPortOptions { Width = 0, Height = 0 },
                Args = new string[] { "" }
            });
            result = new CrawlResult(settings.SeedUrl.ToString());
        }
        private async Task UninitializeCrawl()
        {
            await browser.CloseAsync();
        }
        private async Task CrawlPageAsync(CrawlPage pageToCrawl, uint depth = 0)
        {
            if (depth > settings.MaxDepth)
            {
                return;
            }

            CrawlPage crawledPage;
            if(result.CrawledPages.TryGetValue(pageToCrawl.Url.ToString(), out crawledPage))
            {
                Console.WriteLine(string.Format("Resuing route: {0}", pageToCrawl.Url));

                pageToCrawl.Title = crawledPage.Title;
                pageToCrawl.Img = crawledPage.Img;
                pageToCrawl.Dom = crawledPage.Dom;
                pageToCrawl.ChildPages = crawledPage.ChildPages;

                foreach(var childPage in pageToCrawl.ChildPages)
                {
                    if(result.CrawledPages.TryGetValue(childPage.Url.ToString(), out crawledPage))
                    {
                        childPage.Title = crawledPage.Title;
                        childPage.Img = crawledPage.Img;
                    }
                }
                return;
            }
            else
            {
                Console.WriteLine(string.Format("Loading page: {0}", pageToCrawl.Url));

                Page newPage = await browser.NewPageAsync();
                await newPage.GoToAsync(pageToCrawl.Url.ToString(), WaitUntilNavigation.Networkidle2);

                var anchorsJsToken = await newPage.EvaluateFunctionAsync(_collectAllAnchorsDeepScriptJs, true);
                var hrefs = anchorsJsToken.ToObject<HashSet<string>>();
                hrefs = hrefs.Where(a => a != settings.SeedUrl.ToString()).ToHashSet();

                var title = await newPage.EvaluateExpressionAsync("document.title");
                pageToCrawl.Title = title.ToString();

                foreach(var href in hrefs)
                {
                    pageToCrawl.ChildPages.Add(new CrawlPage(new Uri(href)));
                }

                if(settings.ScreenShots)
                {
                    string imageFile = string.Format(@"{0}_screenshot.png", pageToCrawl.Guid.ToString().Replace("-", ""));
                    Bitmap bitMap = new Bitmap(Image.FromStream(await newPage.ScreenshotStreamAsync()));
                    imageFile = Path.Join(result.OutputFolder.FullName, imageFile);
                    bitMap.Save(imageFile);
                    pageToCrawl.Img = new CrawlImg(bitMap, imageFile);
                }

                if(settings.DumpDom)
                {
                    string domFile = string.Format(@"{0}_dom.html", pageToCrawl.Guid.ToString().Replace("-", ""));
                    var domHtml = await newPage.GetContentAsync();
                    domFile = Path.Join(result.OutputFolder.FullName, domFile);
                    File.WriteAllText(domFile, domHtml);
                    pageToCrawl.Dom = new CrawlDom(domHtml, domFile);
                }

                result.CrawledPages.Add(pageToCrawl.Url.ToString(), pageToCrawl);
                await newPage.CloseAsync();
            }

            foreach(var childPage in pageToCrawl.ChildPages)
            {
                await CrawlPageAsync(childPage, depth + 1);
            }
        }
        public async Task RunAsync(CrawlSettings settings)
        {
            await InitializeCrawl(settings);
            await CrawlPageAsync( new CrawlPage(new Uri(settings.SeedUrl.ToString())));
            result.WriteResults();
            await UninitializeCrawl();
        }
    }
}
