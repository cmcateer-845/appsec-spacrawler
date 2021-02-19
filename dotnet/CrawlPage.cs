using System;
using System.Collections.Generic;
using System.Text;

namespace SpaCrawler
{
    internal class CrawlPage
    {
        public Guid Guid { get; }
        public Uri Url { get; private set; }
        public string Title { get; set; }
        public CrawlImg Img { get; set; }
        public CrawlDom Dom { get; set; }
        public HashSet<CrawlPage> ChildPages { get; set; }
        public CrawlPage(Uri url, string title = "", CrawlImg img = null, CrawlDom dom = null, HashSet<CrawlPage> childPages = null)
        {
            Guid = Guid.NewGuid();
            Url = url;
            Title = title;

            if(img != null)
            {
                Img = img;
            }
            else
            {
                Img = new CrawlImg();
            }

            if(dom != null)
            {
                Dom = dom;
            }
            else
            {
                Dom = new CrawlDom();
            }

            if(childPages != null)
            {
                ChildPages = childPages;
            }
            else
            {
                ChildPages = new HashSet<CrawlPage>();
            }
        }
    }
}
