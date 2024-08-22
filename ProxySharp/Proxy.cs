using System;
using System.Collections.Generic;
using System.Linq;

namespace ProxySharp
{
    /// <summary>
    /// Holds all of the logic for manipulating proxies. 
    /// </summary>
    public class Proxy
    {
        /// <summary>
        /// The queue that holds all of the proxies returned from the scraper
        /// </summary>
        private static List<string> queue = new List<string>();

        /// <summary>
        /// A list of proxies that are no longer in the queue.
        /// </summary>
        private static readonly List<string> usedProxies = new List<string>();


        /// <summary>
        /// The static constructor that starts the scraper and adds proxies to the queue
        /// </summary>
        static Proxy()
        {
            queue = Scrape.ScrapeProxies();
        }

        /// <summary>
        /// Gets a list of proxies that are currently in the queue.
        /// </summary>
        /// <returns>return a list of queued proxies</returns>
        public static List<string> GetProxies()
        {
            return queue;
        }

        /// <summary>
        /// Gets a single proxy from the queue.
        /// </summary>
        /// <returns>A proxy server IP and Port address.</returns>
        public static string GetSingleProxy()
        {
            var proxy = queue.First();
            return proxy;
        }

        /// <summary>
        /// Gets a randomly choosed single proxy from the queue.
        /// </summary>
        /// <returns>A proxy server IP and Port address.</returns>
        public static string GetSingleRandomProxy()
        {
            Random rnd = new Random();

            int randomIndex = rnd.Next(1, queue.Count);
            var temp = queue[randomIndex];

            queue.RemoveAt(randomIndex); // By moving it to the first index, the methode `PopProxy` can still be used.
            queue.Insert(0, temp);

            var proxy = queue.First();
            return proxy;
        }

        /// <summary>
        /// Gets a list of previously used proxies. The lower the index, the older the proxy.
        /// </summary>
        /// <returns>return a list of used proxies</returns>
        public static List<string> GetUsedProxies()
        {
            return usedProxies;
        }

        /// <summary>
        /// Clears the queue then adds a fresh list of proxies to the queue.
        /// </summary>
        public static void RenewQueue()
        {
            usedProxies.AddRange(queue);
            queue.Clear();
            queue = Scrape.ScrapeProxies(null, false);
        }

        /// <summary>
        /// Clears the queue, then adds a fresh, filtered list of proxies based on the specified country code. Allows filtering by or excluding proxies from the specified country.
        /// </summary>
        /// <param name="countryCode">The two-letter code of the country to filter by (e.g., "US", "JP", "NZ").</param>
        /// <param name="excludeCountry">Set to true to exclude all proxies that match the filter; set to false to include only proxies that match the filter.</param>
        /// <example>
        /// RenewFilteredProxies("US", false);
        /// // Renews a list of proxies from the United States only.
        /// 
        /// RenewFilteredProxies("JP", true);
        /// // Renews a list of proxies from all countries except Japan.
        /// </example>
        public static void RenewFilteredProxies(string countryCode, bool excludeCountry)
        {
            usedProxies.AddRange(queue);
            queue.Clear();
            queue = Scrape.ScrapeProxies(countryCode, excludeCountry);
        }

        /// <summary>
        /// Gets the index of a proxy.
        /// </summary>
        /// <param name="proxy">the proxy that you want the index of</param>
        /// <returns>return a integer that indicates the index of the given proxy</returns>
        public static int GetIndex(string proxy)
        {
            return queue.IndexOf(proxy);
        }

        /// <summary>
        /// Adds the specified proxy to the queue.
        /// </summary>
        /// <param name="ip">The ip address of the proxy to add to the queue.</param>
        /// <param name="port">The port number of the proxy to add to the queue.</param>
        public static void AddProxy(string ip, string port)
        {
            queue.Add(ip + ":" + port);
        }

        /// <summary>
        /// Removes the first proxy in the queue.
        /// </summary>
        public static void PopProxy()
        {
            var proxy = queue.First();
            usedProxies.Add(proxy);
            queue.Remove(proxy);
        }
    }
}
