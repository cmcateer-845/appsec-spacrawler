using System;
using System.IO;
using System.Threading.Tasks;

namespace SpaCrawler
{
    class Program
    {
        static void ShowHelp(string errorReason = null)
        {
            if(!string.IsNullOrEmpty(errorReason))
            {
                Console.WriteLine(string.Format("{0} {1}{2}", "Error", errorReason, "\n\n"));
            }

            Console.WriteLine("Usage: SpaCrawler.exe [options] [arguments]\n");
            Console.WriteLine(string.Format("\t{0,-30}\t{1}", "--seedUrl <url>", "The url to start crawling from."));
            Console.WriteLine(string.Format("\t{0,-30}\t{1}", "--depth <depth>", "The max crawl depth."));
            Console.WriteLine(string.Format("\t{0,-30}\t{1}", "--screenShots", "Take a screenshot of each page visited during crawl."));
            Console.WriteLine(string.Format("\t{0,-30}\t{1}", "--dumpDom", "Dump the DOM of each page visited during crawl."));
            Console.WriteLine(string.Format("\t{0,-30}\t{1}", "--headless", "Use headless browser."));
            Console.WriteLine(string.Format("\t{0,-30}\t{1}", "--outputDirectory <directory>", "Output directory of crawl results."));
            Console.WriteLine(string.Format("\t{0,-30}\t{1}", "--playbackFile", "Path to a valid playback file."));
            Console.WriteLine(string.Format("\t{0,-30}\t{1}", "--help", "Show this help page."));
        }
        static async Task Main(string[] args)
        {
            uint depth = 0;
            string outputDirectory = "";
            string playbackFile = "";
            string seedUrl = "";
            bool screenShots = false;
            bool dumpDom = false;
            bool headless = false;

            if (args.Length == 0)
            {
                ShowHelp();
                return;
            }

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--help":
                        ShowHelp();
                        break;

                    case "--seedUrl":
                        i++;
                        if (string.IsNullOrEmpty(args[i]) ||
                            args[i].StartsWith("--"))
                        {
                            ShowHelp("--seedUrl requires a valid url as an argument.");
                            return;
                        }
                        seedUrl = args[i];
                        break;

                    case "--depth":
                        i++;
                        if (string.IsNullOrEmpty(args[i]) ||
                            args[i].StartsWith("--") ||
                            !uint.TryParse(args[i], out depth))
                        {
                            ShowHelp("--depth requires a valid unsigned integer as an argument.");
                            return;
                        }
                        break;

                    case "--outputDirectory":
                        i++;
                        if (string.IsNullOrEmpty(args[i]) ||
                            args[i].StartsWith("--") ||
                            !Directory.Exists(args[i]))
                        {
                            ShowHelp("--outputDirectory requires a valid directory as an argument.");
                            return;
                        }
                        outputDirectory = args[i];
                        break;

                    case "--playbackFile":
                        i++;
                        if (string.IsNullOrEmpty(args[i]) ||
                            args[i].StartsWith("--") ||
                            !File.Exists(args[i]))
                        {
                            ShowHelp("--playbackFile requires a valid path to a playback file as an argument.");
                            return;
                        }
                        playbackFile = args[i];
                        break;

                    case "--screenShots":
                        screenShots = true;
                        break;

                    case "--dumpDom":
                        dumpDom = true;
                        break;

                    case "--headless":
                        headless = true;
                        break;

                    default:
                        ShowHelp();
                        break;
                }
            }

            Console.WriteLine("Command line arguments:");
            Console.WriteLine(string.Format("seedUrl={0}", seedUrl));
            Console.WriteLine(string.Format("depth={0}", depth));
            Console.WriteLine(string.Format("outputDirectory={0}", outputDirectory));
            Console.WriteLine(string.Format("playbackFile={0}", playbackFile));
            Console.WriteLine(string.Format("headless={0}", headless ? "true" : "false"));
            Console.WriteLine(string.Format("screenShots={0}", screenShots ? "true" : "false"));
            Console.WriteLine(string.Format("dumpDom={0}", dumpDom ? "true" : "false"));

            try
            {
                Crawler crawler = new Crawler();
                CrawlSettings crawlSettings = new CrawlSettings(new Uri(seedUrl), outputDirectory, playbackFile, depth, headless, screenShots, dumpDom);
                await crawler.RunAsync(crawlSettings);

                Console.WriteLine(
                    string.Format("Success: Crawl of {0} completed successfully - Report written to {1}",
                    seedUrl,
                    outputDirectory));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(
                    string.Format("Error: Crawl of {0} ended prematurley - Details: {1}",
                    seedUrl,
                    e.Message));
            }
        }
    }
}
