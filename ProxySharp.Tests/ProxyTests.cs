namespace ProxySharp.Tests
{
    public class ProxyTests
    {
        [Fact]
        public void GetProxiesTest()
        {
            Assert.NotEmpty(Proxy.GetProxies());
            Assert.InRange(Proxy.GetProxies().Count, 1, 1000);
            Assert.Matches(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}", Proxy.GetProxies().First());
            Assert.IsType<List<string>>(Proxy.GetProxies());
        }

        [Fact]
        public void GetSingleProxyTest()
        {
            Assert.NotEmpty(Proxy.GetSingleProxy());
            Assert.IsType<string>(Proxy.GetSingleProxy());
            Assert.Matches(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}", Proxy.GetSingleProxy());
            Assert.Equal(Proxy.GetSingleProxy(), Proxy.GetSingleProxy());
        }

        [Fact]
        public void GetSingleRandomProxyTest()
        {
            Assert.NotEmpty(Proxy.GetSingleProxy());
            Assert.IsType<string>(Proxy.GetSingleProxy());
            Assert.Matches(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}", Proxy.GetSingleProxy());
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
            Assert.Equal(0, Proxy.GetIndex(prox2));
            Assert.Equal(Proxy.GetUsedProxies().Last(), prox1);
        }
    }
}