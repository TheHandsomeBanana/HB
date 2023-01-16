using HB.Utilities.Services.Caching;

namespace HB.Services.Caching.Tests {
    [TestClass]
    public class CachingServiceTests {
        private readonly TestCache cache = new TestCache("test", 10, DateTime.Now);

        [TestMethod]
        public void TestJson() {
            CachingService cachingService = new CachingService();

            cachingService.AddOrUpdate("Test", new Cache(cache, CacheType.Json, 10));
            Assert.IsTrue(cachingService.CacheTable.ContainsKey("Test"));
            Thread.Sleep(12000);
            Assert.IsFalse(cachingService.CacheTable.ContainsKey("Test"));
        }

        [TestMethod]
        public void TestXml() {
            CachingService cachingService = new CachingService();

            cachingService.AddOrUpdate("Test", new Cache(cache, CacheType.Xml, 10));
            Assert.IsTrue(cachingService.CacheTable.ContainsKey("Test"));
            Thread.Sleep(12000);
            Assert.IsFalse(cachingService.CacheTable.ContainsKey("Test"));
        }

        [TestMethod]
        public void TestBinary() {
            CachingService cachingService = new CachingService();

            cachingService.AddOrUpdate("Test", new Cache(cache, CacheType.Binary, 10));
            Assert.IsTrue(cachingService.CacheTable.ContainsKey("Test"));
            Thread.Sleep(12000);
            Assert.IsFalse(cachingService.CacheTable.ContainsKey("Test"));

            cachingService.Reload("Test");


        }

    }

    [Serializable]
    public class TestCache {
        public string Message { get; set; }
        public int Counter { get; set; }
        public DateTime Timestamp { get; set; }

        public TestCache(string message, int counter, DateTime timestamp) {
            Message = message;
            Counter = counter;
            Timestamp = timestamp;
        }
    }
}