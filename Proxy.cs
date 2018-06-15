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
        /// the queue the holds all of the proxies returned from the scraper
        /// </summary>
        private static List<string> queue = new List<string>();

        /// <summary>
        /// the static constructor that starts the scraper and adds proxies to the queue
        /// </summary>
        static Proxy()
        {
            queue = Scrape.ScrapeProxies();
        }

        /// <summary>
        /// Gets a list of proxies that are currently in the queue.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetProxies()
        {
            return queue;
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
        /// Clears the queue and adds a fresh list of proxies to the queue.
        /// </summary>
        public static void RenewQueue()
        {
            queue.Clear();
            queue = Scrape.ScrapeProxies();
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
            queue.Remove(proxy);
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

    }
}
