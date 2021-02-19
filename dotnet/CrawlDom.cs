using System;
using System.Collections.Generic;
using System.Text;

namespace SpaCrawler
{
    internal class CrawlDom
    {
        public string Html { get; }
        public string FilePath { get; }

        public CrawlDom()
        {
            Html = "";
            FilePath = "";
        }
        public CrawlDom(string html, string filePath)
        {
            Html = html;
            FilePath = filePath;
        }
    }
}
