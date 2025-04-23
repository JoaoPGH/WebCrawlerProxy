# WebCrawlerProxy

## Este projeto é uma aplicação console em .NET que implementa um WebCrawler multithreaded para coletar dados de proxies do site proxyservers.pro.

Funcionalidades

* Acessa automaticamente páginas com proxies.

* Extrai dados como IP, Porta, País e Protocolo.

* Salva os dados em um arquivo .json.

* Salva uma cópia .html de cada página visitada.

* Armazena logs da execução no banco de dados SQLite.

* Limita a execução concorrente a no máximo 3 threads.