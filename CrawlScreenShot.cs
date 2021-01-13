using System.Drawing;

namespace SpaCrawler
{
    internal class CrawlScreenShot
    {
        private Bitmap _bitMap { get; set; }
        private string _filePath { get; set; }
        public Bitmap BitMap { get => _bitMap; }
        public string FilePath { get => _filePath; }
        public CrawlScreenShot()
        {
            _bitMap = null;
            _filePath = "";
        }
        public CrawlScreenShot(Bitmap bitMap, string filePath)
        {
            _bitMap = bitMap;
            _filePath = filePath;
        }
    }
}