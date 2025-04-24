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
                    
                    if (cells?.Count >= 7)
                    {
                        proxies.Add(new ProxyData
                        {
                            Ip = cells[1].InnerText.Trim(),
                            Port = cells[2].SelectSingleNode(".//span[@class='port']")?.GetAttributeValue("data-port", "").Trim(),
                            Country = cells[3].InnerText.Trim(),
                            Protocol = cells[6].InnerText.Trim()
                        });
                    }
                }
            }

            return proxies;
        }
    }
}