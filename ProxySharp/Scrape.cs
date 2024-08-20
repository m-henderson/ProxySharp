using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProxySharp
{
    /// <summary>
    /// Responsible for all of the logic that us used for scraping proxies. 
    /// </summary>
    public class Scrape
    {
        /// <summary>
        /// Scrapes a list of proxies by country code. If the country code is not set or not find the list will not be filtered 
        /// </summary>
        /// <param name="countryCode">The country code choosen to filters list. (optionnal)</param>
        /// <param name="excludeCountry">Set to true to exclude all proxies that match the filter; set to false to include only proxies that match the filter. (optionnal)</param>
        /// <returns>A list of proxies.</returns>
        public static List<string> ScrapeProxies(string countryCode = null, bool excludeCountry = false)
        {
            string url = "https://free-proxy-list.net/";
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(url);
            var proxyTable = doc.DocumentNode.SelectSingleNode("//table");

            var proxyQueue = new List<string>();

            var ip = ScrapeProxieIp(proxyTable);
            var ports = ScrapeProxiePort(proxyTable);
            var codes = ScrapingProxyCountryCode(proxyTable);
            var proxy = "";

            foreach (var nw in ip.Zip(ports, Tuple.Create))
            {
                proxy = nw.Item1 + ":" + nw.Item2;
                proxyQueue.Add(proxy);
            }

            if (countryCode != null && codes.Contains(countryCode))
            {
                proxyQueue = FilterProxy(proxyQueue, codes, countryCode, excludeCountry);
            }
            return proxyQueue;
        }

        /// <summary>
        /// Gets all of the Proxy Ip Addreses from the scraped proxy table.
        /// </summary>
        /// <param name="proxyTable">The scraped table that holds all of the proxy information.</param>
        /// <returns>A list of proxie ip addreses.</returns>
        private static List<string> ScrapeProxieIp(HtmlNode proxyTable)
        {
            var proxyIp = new List<string>();

            var allPorts = proxyTable.SelectNodes("tbody/tr/td[1]");

            foreach (var port in allPorts)
            {
                proxyIp.Add(port.InnerHtml);
            }

            return proxyIp;
        }

        /// <summary>
        /// Gets all of the Proxy port numbers from the scraped proxy table.
        /// </summary>
        /// <param name="proxyTable">The scraped table that holds all of the proxy information.</param>
        /// <returns>A list of proxie ports.</returns>
        private static List<string> ScrapeProxiePort(HtmlNode proxyTable)
        {
            var proxyPorts = new List<string>();

            var allPorts = proxyTable.SelectNodes("tbody/tr/td[2]");

            foreach (var port in allPorts)
            {
                proxyPorts.Add(port.InnerHtml);
            }

            return proxyPorts;
        }

        /// <summary>
        /// Gets all of the Proxy Country Codes from the scraped proxy table.
        /// </summary>
        /// <param name="proxyTable">The scraped table that holds all of the proxy information.</param>
        /// <returns>A list of country codes.</returns>
        private static List<string> ScrapingProxyCountryCode(HtmlNode proxyTable)
        {
            var proxyCode = new List<string>();

            var allCodes = proxyTable.SelectNodes("tbody/tr/td[3]");

            foreach (var code in allCodes)
            {
                proxyCode.Add(code.InnerHtml);
            }

            return proxyCode;
        }

        /// <summary>
        /// Filter the proxies by country code. Remove all proxies that do not match the filter.
        /// </summary>
        /// <param name="proxyQueue">The list of gathered proxies.</param>
        /// <param name="codes">The list of gathered country codes.</param>
        /// <param name="countryCode">The two-letter code of the country to filter by (e.g., "US", "JP", "NZ").</param>
        /// <param name="excludeCountry">Set to true to exclude all proxies that match the filter; set to false to include only proxies that match the filter.</param>
        /// <returns>A list of filtered proxies</returns>
        private static List<string> FilterProxy(List<string> proxyQueue, List<string> codes, string countryCode, bool excludeCountry)
        {
            List<int> indexes = new List<int>();

            switch (excludeCountry)
            {
                case false:
                    for (int i = 0; i < codes.Count; i++)
                    {
                        if (codes[i] != countryCode)
                        {
                            indexes.Add(i);
                        }
                    }
                    break;
                case true:
                    for (int i = 0; i < codes.Count; i++)
                    {
                        if (codes[i] == countryCode)
                        {
                            indexes.Add(i);
                        }
                    }
                    break;
            }

            indexes.Reverse(); // Needed to avoids index out of range errors or non-targeted proxies deletions.

            foreach (var index in indexes)
            {
                proxyQueue.RemoveAt(index);
            }

            return proxyQueue;
        }
    }
}
