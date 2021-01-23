using System;
using System.Collections.Generic;
using System.Text;

namespace SpaCrawler
{
    internal class CrawlPage
    {
        private Guid _guid { get; set; }
        private Uri _url { get; set; }
        private string _title { get; set; }
        private CrawlScreenShot _screenShot { get; set; }
        private HashSet<CrawlPage> _childPages { get; set; }
        public Guid Guid { get => _guid; }
        public Uri Url
        {
            get => _url;
            set => _url = value;
        }
        public string Title
        {
            get => _title;
            set => _title = value;
        }
        public CrawlScreenShot ScreenShot
        {
            get => _screenShot;
            set => _screenShot = value;
        }
        public HashSet<CrawlPage> ChildPages
        {
            get => _childPages;
            set => _childPages = value;
        }
        public bool AddChild(CrawlPage child)
        {
            return _childPages.Add(child);
        }
        public bool RemoveChild(CrawlPage child)
        {
            return _childPages.Remove(child);
        }
        public bool HasChild(CrawlPage child)
        {
            return _childPages.Contains(child);
        }
        public CrawlPage()
        {
            _guid = Guid.NewGuid();
            _screenShot = new CrawlScreenShot();
            _childPages = new HashSet<CrawlPage>();
        }
        public CrawlPage(Uri url)
        {
            _guid = Guid.NewGuid();
            _url = url;
            _screenShot = new CrawlScreenShot();
            _childPages = new HashSet<CrawlPage>();
        }
        public CrawlPage(Uri url, string title)
        {
            _guid = Guid.NewGuid();
            _url = url;
            _title = title;
            _screenShot = new CrawlScreenShot();
            _childPages = new HashSet<CrawlPage>();
        }
        public CrawlPage(Uri url, string title, CrawlScreenShot screenShot)
        {
            _guid = Guid.NewGuid();
            _url = url;
            _title = title;
            _screenShot = screenShot;
            _childPages = new HashSet<CrawlPage>();
        }
        public CrawlPage(Uri url, string title, CrawlScreenShot screenShot, HashSet<CrawlPage> childPages)
        {
            _guid = Guid.NewGuid();
            _url = url;
            _title = title;
            _screenShot = screenShot;
            _childPages = childPages;
        }
    }
}
