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
        /// Scrapes a raw list of proxies.
        /// </summary>
        /// <returns>A list of proxies.</returns>
        public static List<string> ScrapeProxies()
        {
            var proxiesDataTables = GetProxiesData();
            var proxyQueue = ProxyQueuing(proxiesDataTables);

            return proxyQueue;
        }


        /// <summary>
        /// Scrapes the proxy data from the website. 
        /// </summary>
        /// <returns>A list of tuples that contain the proxy, country code, and port.</returns>
        public static List<(string, string, string)> GetProxiesData()
        {
            List<(string, string, string)> proxiesDataTable = new List<(string, string, string)>();

            string url = "https://free-proxy-list.net/";
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(url);
            var proxyTable = doc.DocumentNode.SelectSingleNode("//table");

            var ip = ScrapeProxieIp(proxyTable);
            var ports = ScrapeProxiePort(proxyTable);
            var codes = ScrapingProxyCountryCode(proxyTable);
            var proxy = "";

            int i = 0;
            foreach (var nw in ip.Zip(ports, Tuple.Create))
            {
                proxy = nw.Item1 + ":" + nw.Item2;
                proxiesDataTable.Add((proxy, codes[i], ports[i]));
                i++;
            }

            return proxiesDataTable;
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
        /// Method to filter the proxies by country code, port, or both. Include only or exclude proxies that match filter.
        /// </summary>
        /// <param name="filterType">The type of filter : 0 = Coutry filter, 1 = Port fliter, 2 = Both filter</param>
        /// <param name="value1">The two-letter code of the country to filter by (e.g., "US", "JP", "NZ") or the port to filter.</param>
        /// <param name="exclude1">Set to true to exclude all proxies that match the filter; set to false to include only proxies that match the filter.</param>
        /// <param name="value2">(Optional) The port to filter. Used only with "filterType = 2"</param>
        /// <param name="exclude2">(Optional) Set to true to exclude all port that match the filter; set to false to include only proxies that match the filter. Used only with "filterType = 2"</param>
        /// <returns>A list of filtered proxies</returns>
        public static List<string> ScrapeFilteredProxy(int filterType = 0, string value1 = "US", bool exclude1 = false, string value2 = "80", bool exclude2 = false)
        {
            var proxiesDataTables = GetProxiesData();

            switch (filterType)
            {
                case 0:
                    FilterCountry(proxiesDataTables, value1, exclude1);
                    break;

                case 1:
                    FilterPort(proxiesDataTables, value1, exclude1);
                    break;

                case 2:
                    FilterCountry(proxiesDataTables, value1, exclude1);
                    FilterPort(proxiesDataTables, value2, exclude2);
                    break;
            }

            var proxyQueue = ProxyQueuing(proxiesDataTables);
            return proxyQueue;
        }

        /// <summary>
        /// Method to filter the proxies by country code. Include only or exclude proxies that match filter.
        /// </summary>
        /// <param name="proxiesDataTables">The tuple list of proxies, country codes, and ports.</param>
        /// <param name="value">The country code filter</param>
        /// <param name="exclude">Bool used to determine if the proxies that match the filter should be included or excluded.</param>
        private static void FilterCountry(List<(string, string, string)> proxiesDataTables, string value, bool exclude)
        {
            List<int> IndexesList = new List<int>();
            switch (exclude)
            {
                case false:
                    foreach (var item in proxiesDataTables)
                    {
                        if (item.Item2 != value)
                        {
                            IndexesList.Add(proxiesDataTables.IndexOf(item));
                        }
                    }
                    break;

                case true:
                    foreach (var item in proxiesDataTables)
                    {
                        if (item.Item2 == value)
                        {
                            IndexesList.Add(proxiesDataTables.IndexOf(item));
                        }
                    }
                    break;
            }

            IndexesList.Reverse(); // Reverse the list to avoid index out of range error.
            foreach (var index in IndexesList)
            {
                proxiesDataTables.RemoveAt(index);
            }
        }

        /// <summary>
        /// Method to filter the proxies by port. Include only or exclude proxies that match filter.
        /// </summary>
        /// <param name="proxiesDataTables">The tuple list of proxies, country codes, and ports.</param>
        /// <param name="value">The port filter</param>
        /// <param name="exclude">Bool used to determine if the proxies that match the filter should be included or excluded.</param>
        private static void FilterPort(List<(string, string, string)> proxiesDataTables, string value, bool exclude)
        {
            List<int> IndexesList = new List<int>();
            switch (exclude)
            {
                case false:
                    foreach (var item in proxiesDataTables)
                    {
                        if (item.Item3 != value)
                        {
                            IndexesList.Add(proxiesDataTables.IndexOf(item));
                        }
                    }
                    break;

                case true:
                    foreach (var item in proxiesDataTables)
                    {
                        if (item.Item3 == value)
                        {
                            IndexesList.Add(proxiesDataTables.IndexOf(item));
                        }
                    }
                    break;
            }

            IndexesList.Reverse(); // Reverse the list to avoid index out of range error.
            foreach (var index in IndexesList)
            {
                proxiesDataTables.RemoveAt(index);
            }
        }

        /// <summary>
        /// Method to add all proxies to the queue whitout filtering.
        /// </summary>
        /// <param name="proxiesDataTables">The tuple list of proxies, country codes, and ports.</param>
        /// <returns></returns>
        private static List<string> ProxyQueuing(List<(string, string, string)> proxiesDataTables)
        {
            var proxyQueue = new List<string>();

            foreach ((string, string, string) proxy in proxiesDataTables)
            {
                proxyQueue.Add(proxy.Item1);
            }

            return proxyQueue;
        }
    }
}
                                                        