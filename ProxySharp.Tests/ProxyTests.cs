namespace ProxySharp.Tests
{
    public class ProxyTests
    {
        [Fact]
        public void GetProxiesTest()
        {
            Assert.Contains("0.0.0.0:80", Proxy.GetProxies());
        }

        [Fact]
        public void GetSingleProxyTest()
        {
            Assert.NotEmpty(Proxy.GetSingleProxy());
        }

        [Fact]
        public void GetSingleRandomProxyTest()
        {
            Assert.NotEqual(Proxy.GetSingleProxy(), Proxy.GetSingleRandomProxy());
        }

        [Fact]
        public void GetUsedProxiesTest()
        {
            var temp = Proxy.GetProxies();
            Proxy.RenewQueue();
            var temp2 = Proxy.GetUsedProxies();

            for (int i = 0; i < temp.Count; i++)
            {
                Assert.Equal(temp[i], temp2[i]);
            }
        }

        [Fact]
        public void RenewQueueTest()
        {
            var temp = Proxy.GetProxies();
            Proxy.RenewQueue();
            var temp2 = Proxy.GetUsedProxies();

            for (int i = 0; i < temp.Count; i++)
            {
                Assert.NotEqual(temp[i], temp2[i]);
            }
        }

        [Fact]
        public void RenewFilteredProxiesTest()
        {
            var temp = Proxy.GetProxies();
            Proxy.RenewQueue();
            var temp2 = Proxy.GetUsedProxies();

            for (int i = 0; i < temp.Count; i++)
            {
                Assert.NotEqual(temp[i], temp2[i]);
            }
        }

        [Fact]
        public void GetIndexTest()
        {
            var proxy = Proxy.GetSingleProxy();
            Assert.Equal(0, Proxy.GetIndex(proxy));
        }

        [Fact]
        public void AddProxyTest()
        {
            Proxy.AddProxy("1.2.3.4", "5");
            Assert.Equal("1.2.3.4:5", Proxy.GetProxies().Last());
        }

        [Fact]
        public void PopProxyTest()
        {
            var prox1 = Proxy.GetSingleProxy();
            Proxy.PopProxy();
            var prox2 = Proxy.GetSingleProxy();
            Assert.NotEqual(prox1, prox2);
        }
    }
}