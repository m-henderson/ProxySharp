# ProxySharp
An easy to use .NET library for generating usable, random proxy servers.

In the usage example below we make a post request look like it is coming from another server instead of our actual machine.
### Usage Example
![](https://i.imgur.com/6sX9yXG.png)

---
## Install
```
Install-Package ProxySharp -Version 2.0.1
```
## Functions
- `GetProxies()` : Gets a list of proxies that are currently in the queue.
- `GetSingleProxy()` : Gets a single proxy from the queue.
- `GetSingleRandomProxy()` : Gets a single randomly choosed proxy from the queue.
- `GetUsedProxies()` :  Gets a list of previously used proxies. The lower the index, the older the proxy.
- `RenewQueue()` : Clears the queue then adds a fresh list of proxies to the queue.
- `FilterQueueByCountry(string value, bool exclude)` : Filter the actual queue by country based on given values.
- `FilterQueueByPort(string value, bool exclude)` : Filter the actual queue by anonymity based on given values.
- `FilterQueueByAnonymityLevel(int value, bool exclude)` : Filter the actual queue by anonymity level based on given values.
- `FilterQueueByHttps(bool value, bool exclude)` : Filter the actual queue by Https support based on given values.
- `GetIndex(string proxy)` : Gets the index of a proxy.
- `AddProxy(string ip, string port)` : Adds the specified proxy to the queue.
- `PopProxy()` : Removes the first proxy in the queue.

## Specifications
- All functions for filtering and renew uses a list of tuples from `Scrape.cs`. This list store all data related to the proxies. These proxies in `queue` are extracted from that list. 
- `RenewQueue()` is the only function that renew this list of tuples, so it is the only way to get fresh proxies or remove applied filters.
- `FilterQueueByXXX()` does not renew the list of tuples, it only filters the actual queue with the criteria. Allow to chain multiple filters.

## Dependencies
- [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack/) [v1.11.62]
- [NETStandard.Library](https://www.nuget.org/packages/NETStandard.Library) [v2.0.3]
## Contributor Community
- [Discord](https://discord.gg/F77g42ZNFa)
