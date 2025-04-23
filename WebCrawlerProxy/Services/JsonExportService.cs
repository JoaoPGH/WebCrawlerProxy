using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using WebCrawlerProxy.Models;

namespace WebCrawlerProxy.Services
{
    public class JsonExportService
    {
        public string SaveToJson(List<ProxyData> proxies, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var fileName = $"proxies_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var filePath = Path.Combine(directoryPath, fileName);

            var json = JsonConvert.SerializeObject(proxies, Formatting.Indented);
            File.WriteAllText(filePath, json);

            return filePath;
        }
    }
}