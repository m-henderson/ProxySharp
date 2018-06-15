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
        /// Scrapes a list of proxies. 
        /// </summary>
        /// <returns>List of proxies.</returns>
        public static List<string> ScrapeProxies()
        {
            string url = "https://free-proxy-list.net/";
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(url);
            var proxyTable = doc.DocumentNode.SelectSingleNode("//table");

            var proxyQueue = new List<string>();

            var ip = ScrapeProxieIp(proxyTable);
            var ports = ScrapeProxiePort(proxyTable);
            var proxy = "";

            foreach (var nw in ip.Zip(ports, Tuple.Create))
            {
                proxy = nw.Item1 + ":" + nw.Item2;
                proxyQueue.Add(proxy);
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

    }
}
