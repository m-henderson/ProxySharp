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
        /// The queue that holds all of the proxies data returned from the scraper
        /// </summary>
        private static readonly List<(string, string, string, int, bool)> ProxiesDataTable = new List<(string, string, string, int, bool)>();

        /// <summary>
        /// Scrapes the proxy data from the website.
        /// </summary>
        /// <returns>A list of tuples that contain the proxy, country code, and port.</returns>
        internal static void GatherProxiesData()
        {
            string url = "https://free-proxy-list.net/";
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(url);
            var proxyTable = doc.DocumentNode.SelectSingleNode("//table");

            var ip = ScrapeProxieIp(proxyTable);
            var ports = ScrapeProxiePort(proxyTable);
            var codes = ScrapingProxyCountryCode(proxyTable);
            var anonymity = ScrapingProxyAnonymity(proxyTable);
            var https = ScrapingProxyHttps(proxyTable);
            var proxy = "";

            int i = 0;
            foreach (var nw in ip.Zip(ports, Tuple.Create))
            {
                proxy = nw.Item1 + ":" + nw.Item2;
                ProxiesDataTable.Add((proxy, codes[i], ports[i], anonymity[i], https[i]));
                i++;
            }
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
        /// Gets all of the Proxy Anonymity level from the scraped proxy table.
        /// </summary>
        /// <param name="proxyTable">The scraped table that holds all of the proxy information.</param>
        /// <returns>A list of anonymity level.</returns>
        private static List<int> ScrapingProxyAnonymity(HtmlNode proxyTable)
        {
            var proxyAnonymity = new List<int>();

            var allAnonymity = proxyTable.SelectNodes("tbody/tr/td[5]");

            foreach (var anonymity in allAnonymity)
            {
                if (anonymity.InnerHtml == "elite proxy")
                {
                    proxyAnonymity.Add(1);
                }
                else if (anonymity.InnerHtml == "anonymous")
                {
                    proxyAnonymity.Add(2);
                }
                else if (anonymity.InnerHtml == "transparent")
                {
                    proxyAnonymity.Add(3);
                }
            }

            return proxyAnonymity;
        }

        /// <summary>
        /// Gets all of the Proxy Https support from the scraped proxy table.
        /// </summary>
        /// <param name="proxyTable"></param>
        /// <returns></returns>
        private static List<bool> ScrapingProxyHttps(HtmlNode proxyTable)
        {
            var proxyHttps = new List<bool>();

            var allHttps = proxyTable.SelectNodes("tbody/tr/td[7]");

            foreach (var https in allHttps)
            {
                if (https.InnerHtml == "yes")
                {
                    proxyHttps.Add(true);
                }
                else
                {
                    proxyHttps.Add(false);
                }
            }

            return proxyHttps;
        }

        /// <summary>
        /// Method to filter the proxies by country code. Include only or exclude proxies that match filter.
        /// </summary>
        /// <param name="filter">The country code filter</param>
        /// <param name="exclude">Bool used to determine if the proxies that match the filter should be included or excluded.</param>
        internal static void FilterProxiesDataTableByCountry(string filter, bool exclude)
        {
            List<int> IndexesList = new List<int>();

            switch (exclude)
            {
                case false:
                    foreach (var item in ProxiesDataTable)
                    {
                        if (item.Item2 != filter)
                        {
                            IndexesList.Add(ProxiesDataTable.IndexOf(item));
                        }
                    }
                    break;

                case true:
                    foreach (var item in ProxiesDataTable)
                    {
                        if (item.Item2 == filter)
                        {
                            IndexesList.Add(ProxiesDataTable.IndexOf(item));
                        }
                    }
                    break;
            }

            IndexesList.Reverse(); // Reverse the list to avoid index out of range error.
            foreach (var index in IndexesList)
            {
                ProxiesDataTable.RemoveAt(index);
            }
        }

        /// <summary>                
        /// Method to filter the proxies by port. Include only or exclude proxies that match filter.
        /// </summary>
        /// <param name="filter">The port filter</param>
        /// <param name="exclude">Bool used to determine if the proxies that match the filter should be included or excluded.</param>
        public static void FilterProxiesDataTableByPort(string filter, bool exclude)
        {
            List<int> IndexesList = new List<int>();

            switch (exclude)
            {
                case false:
                    foreach (var item in ProxiesDataTable)
                    {
                        if (item.Item3 != filter)
                        {
                            IndexesList.Add(ProxiesDataTable.IndexOf(item));
                        }
                    }
                    break;

                case true:
                    foreach (var item in ProxiesDataTable)
                    {
                        if (item.Item3 == filter)
                        {
                            IndexesList.Add(ProxiesDataTable.IndexOf(item));
                        }
                    }
                    break;
            }

            IndexesList.Reverse(); // Reverse the list to avoid index out of range error.
            foreach (var index in IndexesList)
            {
                ProxiesDataTable.RemoveAt(index);
            }
        }

        /// <summary>
        /// This method filters the proxies by anonymity level. Include only or exclude proxies that match filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        internal static void FilterProxiesDataTableByAnonymity(int filter, bool exclude)
        {
            List<int> IndexesList = new List<int>();

            switch (exclude)
            {
                case false:
                    foreach (var item in ProxiesDataTable)
                    {
                        if (item.Item4 != filter)
                        {
                            IndexesList.Add(ProxiesDataTable.IndexOf(item));
                        }
                    }
                    break;

                case true:
                    foreach (var item in ProxiesDataTable)
                    {
                        if (item.Item4 == filter)
                        {
                            IndexesList.Add(ProxiesDataTable.IndexOf(item));
                        }
                    }
                    break;
            }
            
            IndexesList.Reverse(); // Reverse the list to avoid index out of range error.
            foreach (var index in IndexesList)
            {
                ProxiesDataTable.RemoveAt(index);
            }
        }

        /// <summary>
        /// This method filters the proxies by https. Include only or exclude proxies that match filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="exclude"></param>
        internal static void FilterProxiesDataTableByHttps(bool filter, bool exclude)
        {
            List<int> IndexesList = new List<int>();

            switch (exclude)
            {
                case false:
                    foreach (var item in ProxiesDataTable)
                    {
                        if (item.Item5 != filter)
                        {
                            IndexesList.Add(ProxiesDataTable.IndexOf(item));
                        }
                    }
                    break;

                case true:
                    foreach (var item in ProxiesDataTable)
                    {
                        if (item.Item5 == filter)
                        {
                            IndexesList.Add(ProxiesDataTable.IndexOf(item));
                        }
                    }
                    break;
            }

            IndexesList.Reverse(); // Reverse the list to avoid index out of range error.
            foreach (var index in IndexesList)
            {
                ProxiesDataTable.RemoveAt(index);
            }
        }

        /// <summary>
        /// Method to add all proxies to the queue whitout filtering.
        /// </summary>
        /// <returns>A list a proxies</returns>
        internal static List<string> ReturnAllProxy()
        {
            var proxyQueue = new List<string>();

            foreach ((string, string, string, int, bool) proxy in ProxiesDataTable)
            {
                proxyQueue.Add(proxy.Item1);
            }

            return proxyQueue;
        }
    }
}
