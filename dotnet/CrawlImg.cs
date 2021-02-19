using System.Drawing;

namespace SpaCrawler
{
    internal class CrawlImg
    {
        public Bitmap BitMap { get; }
        public string FilePath { get; }

        public CrawlImg()
        {
            BitMap = null;
            FilePath = "";
        }
        public CrawlImg(Bitmap bitMap, string filePath)
        {
            BitMap = bitMap;
            FilePath = filePath;
        }
    }
}