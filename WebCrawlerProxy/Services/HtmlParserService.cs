using HtmlAgilityPack;
using WebCrawlerProxy.Models;
using System.Collections.Generic;

namespace WebCrawlerProxy.Services
{
    public class HtmlParserService
    {
        public List<ProxyData> ParseProxies(string html)
        {
            var proxies = new List<ProxyData>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var rows = htmlDoc.DocumentNode.SelectNodes("//table[contains(@class,'table-hover')]/tbody/tr");


            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var cells = row.SelectNodes("td");
                    if (cells?.Count >= 4)
                    {
                        proxies.Add(new ProxyData
                        {
                            Ip = cells[0].InnerText.Trim(),
                            Port = cells[1].InnerText.Trim(),
                            Country = cells[2].InnerText.Trim(),
                            Protocol = cells[3].InnerText.Trim()
                        });
                    }
                }
            }

            return proxies;
        }
    }
}