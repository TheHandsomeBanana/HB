using HB.Common.Serialization;
using HB.Services.Caching;
using HB.Utilities.Services.Caching;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace HB.Services.Caching.Tests {
    [TestClass]
    public class CachingServiceTests {
        private readonly TestCache cache = new TestCache("test", 10, DateTime.Now);

        [TestMethod]
        public void TestJsonAdd() {
            CachingService cachingService = new CachingService();

            cachingService.AddOrUpdate("TestJson", new Cache(cache, SerializerMode.Json, 6));
            Assert.IsTrue(cachingService.CacheTable.ContainsKey("TestJson"));
            Thread.Sleep(6010);
            Assert.IsFalse(cachingService.CacheTable.ContainsKey("TestJson"));

            cachingService.Dispose();
        }


        [TestMethod]
        public void TestJsonGet() {
            CachingService cachingService = new CachingService();

            Cache test = cachingService.GetOrDefault("TestJson") ?? cachingService.GetOrReload("TestJson");
            Assert.IsNotNull(test);

            cachingService.Dispose();
        }

        [TestMethod]
        public void TestXmlAdd() {
            CachingService cachingService = new CachingService();

            cachingService.AddOrUpdate("TestXml", new Cache(cache, SerializerMode.Xml, 1));
            Assert.IsTrue(cachingService.CacheTable.ContainsKey("TestXml"));
            Thread.Sleep(2000);
            Assert.IsFalse(cachingService.CacheTable.ContainsKey("TestXml"));

            cachingService.Dispose();
        }

        [TestMethod]
        public void TestXmlGet() {
            CachingService cachingService = new CachingService();
            Cache? test = cachingService.GetOrDefault("TestXml") ?? cachingService.GetOrReload("TestXml");
            Assert.IsNotNull(test);

            cachingService.Dispose();
        }

        [TestMethod]
        public void TestBinaryAdd() {
            CachingService cachingService = new CachingService();

            cachingService.AddOrUpdate("TestBinary", new Cache(cache, SerializerMode.Binary, 5));
            Assert.IsTrue(cachingService.CacheTable.ContainsKey("TestBinary"));
            Thread.Sleep(5100);
            Assert.IsFalse(cachingService.CacheTable.ContainsKey("TestBinary"));

            cachingService.Dispose();
        }

        [TestMethod]
        public void TestBinaryGet() {
            CachingService cachingService = new CachingService();
            Cache? test = cachingService.GetOrDefault("TestBinary") ?? cachingService.GetOrReload("TestBinary");
            Assert.IsNotNull(test);

            cachingService.Dispose();
        }
    }

    [Serializable]
    public class TestCache {
        public string Message { get; set; }
        public int Counter { get; set; }
        public DateTime Timestamp { get; set; }


        public TestCache() {

        }

        [JsonConstructor]
        public TestCache(string message, int counter, DateTime timestamp) {
            Message = message;
            Counter = counter;
            Timestamp = timestamp;
        }
    }
}