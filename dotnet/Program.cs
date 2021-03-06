﻿using System;
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
            Console.WriteLine(string.Format("\t{0,-30}\t{1}", "--help", "Show this help page."));

            Environment.Exit(0);
        }
        static async Task Main(string[] args)
        {
            uint depth = 0;
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
                        }
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

            if(seedUrl.Length == 0)
            {
                ShowHelp("--seedUrl requires a valid url as an argument.");
                Environment.Exit(1);
            }
            else if(depth <= 0)
            {
                ShowHelp("--depth requires a valid unsigned integer as an argument.");
                Environment.Exit(1);
            }

            Console.WriteLine("Command line arguments:");
            Console.WriteLine(string.Format("seedUrl={0}", seedUrl));
            Console.WriteLine(string.Format("depth={0}", depth));
            Console.WriteLine(string.Format("headless={0}", headless ? "true" : "false"));
            Console.WriteLine(string.Format("screenShots={0}", screenShots ? "true" : "false"));
            Console.WriteLine(string.Format("dumpDom={0}", dumpDom ? "true" : "false"));

            try
            {
                Crawler crawler = new Crawler();
                CrawlSettings crawlSettings = new CrawlSettings(new Uri(seedUrl), depth, headless, screenShots, dumpDom);
                await crawler.RunAsync(crawlSettings);

                Console.WriteLine(string.Format("Success: Crawl of {0} completed successfully.", seedUrl));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(
                    string.Format("Error: Crawl of {0} ended prematurley - Details: {1}",
                    seedUrl,
                    e.Message));

                Environment.Exit(1);
            }

            Environment.Exit(0);
        }
    }
}
