using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebCrawlerProxy.Models;

namespace WebCrawlerProxy.Services
{
    public class WebCrawlerExecutor
    {
        private const string baseUrl = "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc/page/";
        private const int maxConcurrentThreads = 3;

        public async Task ExecuteAsync()
        {

            var rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var htmlDir = Path.Combine(rootPath, "html_pages");
            var jsonDir = Path.Combine(rootPath, "json_output");
            var dbPath = Path.Combine(rootPath, "database.db");

            Directory.CreateDirectory(htmlDir);
            Directory.CreateDirectory(jsonDir);

            var startTime = DateTime.Now;

            var httpClient = new HttpClient();
            var crawlerService = new CrawlerService(httpClient);
            var parserService = new HtmlParserService();
            var jsonService = new JsonExportService();
            var htmlService = new HtmlSnapshotService();
            var logService = new ExecutionLogService($"Data Source={dbPath}");

            var allProxies = new List<ProxyData>();
            var semaphore = new SemaphoreSlim(maxConcurrentThreads);
            var tasks = new List<Task>();
            int pageCount = 0;
            bool hasMorePages = true;

            Console.WriteLine("Iniciando webcrawler...");

            for (int page = 1; hasMorePages; page++)
            {
                await semaphore.WaitAsync();

                int currentPage = page;
                var task = Task.Run(async () =>
                {
                    try
                    {
                        var html = await crawlerService.GetPageHtmlAsync(baseUrl + currentPage);
                        if (string.IsNullOrWhiteSpace(html))
                        {
                            Console.WriteLine($"HTML vazio para a página {currentPage}");
                            hasMorePages = false;
                            return;
                        }

                        var proxies = parserService.ParseProxies(html);

                        if (proxies == null || proxies.Count == 0)
                        {
                            Console.WriteLine($"Nenhum proxy encontrado na página {currentPage}, encerrando...");
                            hasMorePages = false;
                            return;
                        }

                        lock (allProxies)
                        {
                            allProxies.AddRange(proxies);
                            pageCount++;
                        }

                        htmlService.SaveHtmlPage(html, currentPage, htmlDir);
                        Console.WriteLine($"Página {currentPage} processada com {proxies.Count} proxies.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar página {currentPage}: {ex.Message}");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            var jsonPath = jsonService.SaveToJson(allProxies, jsonDir);
            var endTime = DateTime.Now;

            logService.LogExecution(startTime, endTime, pageCount, allProxies.Count, jsonPath);

            Console.WriteLine($"\nCrawler finalizado com sucesso!");
            Console.WriteLine($"Páginas processadas: {pageCount}");
            Console.WriteLine($"Total de proxies: {allProxies.Count}");
            Console.WriteLine($"Arquivo JSON: {jsonPath}");
        }
    }
}
