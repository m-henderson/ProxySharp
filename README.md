# ProxySharp
An easy to use .NET library for generating usable, random proxy servers.

In the usage example below we make a post request look like it is coming from another server instead of our actual machine.
### Usage Example
![](https://i.imgur.com/6sX9yXG.png)

---
## Install
```
Install-Package ProxySharp -Version 2.0.0.0
```
## Functions
- `GetProxies()` : Gets a list of proxies that are currently in the queue.
- `GetSingleProxy()` : Gets a single proxy from the queue.
- `GetSingleRandomProxy()` : Gets a single randomly choosed proxy from the queue.
- `GetUsedProxies()` :  Gets a list of previously used proxies. The lower the index, the older the proxy.
- `RenewQueue()` : Clears the queue then adds a fresh list of proxies to the queue.
- `RenewFilteredProxies(int filterType, string value1, bool exclude1, string value2, bool exclude2)` : Clears the queue, then adds a fresh, filtered list of proxies based on the specified filters. Allows filtering by or excluding proxies. When "filterType = 2", the value1 and exclude1 are used for County and value2 and exclude2 for Port.
- `GetIndex(string proxy)` : Gets the index of a proxy.
- `AddProxy(string ip, string port)` : Adds the specified proxy to the queue.
- `PopProxy()` : Removes the first proxy in the queue.
## Dependencies
- [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack/) [v1.11.62]
- [NETStandard.Library](https://www.nuget.org/packages/NETStandard.Library) [v2.0.3]
## Contributor Community
- [Discord](https://discord.gg/F77g42ZNFa)
