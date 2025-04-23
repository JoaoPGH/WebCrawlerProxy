using System.Net.Http;
using System.Threading.Tasks;

namespace WebCrawlerProxy.Services
{
    public class CrawlerService
    {
        private readonly HttpClient _httpClient;

        public CrawlerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetPageHtmlAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}