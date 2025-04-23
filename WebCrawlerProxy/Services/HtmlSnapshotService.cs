using System;
using System.IO;

namespace WebCrawlerProxy.Services
{
    public class HtmlSnapshotService
    {
        public string SaveHtmlPage(string html, int pageNumber, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var filePath = Path.Combine(directoryPath, $"page_{pageNumber:000}.html");
            File.WriteAllText(filePath, html);

            return filePath;
        }
    }
}