using HB.Common.Serialization;
using HB.Services.Security.Cryptography;
using HB.Services.Security.Cryptography.Keys;

namespace HB.Services.DiskStorage.Tests {
    [TestClass]
    public class MemoryTests {
        private const string jsonFile = "testjson";
        private const string xmlFile = "testxml";

        [TestMethod]
        public void JsonTest() {
            MemoryService memoryService = new MemoryService();
            MemoryObject<TestClass> memoryObj = new MemoryObject<TestClass>(new TestClass("test"), SerializerMode.Json);

            memoryService.WriteMemory(memoryObj, jsonFile);

            TestClass? result = memoryService.ReadMemory<TestClass>(jsonFile, SerializerMode.Json).Deserialize();

            Assert.IsNotNull(result);
            Assert.AreEqual("test", result.Message);
        }

        [TestMethod]
        public void XmlTest() {
            MemoryService memoryService = new MemoryService();
            MemoryObject<TestClass> memoryObj = new MemoryObject<TestClass>(new TestClass("test"), SerializerMode.Xml);

            memoryService.WriteMemory(memoryObj, jsonFile);

            TestClass? result = memoryService.ReadMemory<TestClass>(jsonFile, SerializerMode.Xml).Deserialize();

            Assert.IsNotNull(result);
            Assert.AreEqual("test", result.Message);
        }

        [TestMethod]
        public void NonGenericJsonTest() {
            MemoryService memoryService = new MemoryService();
            MemoryObject memoryObj = new MemoryObject(new TestClass("test"), SerializerMode.Json);

            memoryService.WriteMemory(memoryObj, jsonFile);

            TestClass? result = memoryService.ReadMemory(jsonFile, SerializerMode.Json).Deserialize(typeof(TestClass)) as TestClass;

            Assert.IsNotNull(result);
            Assert.AreEqual("test", result.Message);
        }

        [TestMethod]
        public void JsonTestWithCryptoService() {
            MemoryService memoryService = new MemoryService();
            MemoryObject memoryObj = new MemoryObject(new TestClass("test"), SerializerMode.Json);
            IKey key = KeyGenerator.GenerateAesKey();

            memoryObj.Serialize(key);

            memoryService.WriteMemory(memoryObj, jsonFile);

            TestClass? result = memoryService.ReadMemory(jsonFile, SerializerMode.Json).Deserialize(typeof(TestClass), key) as TestClass;

            Assert.IsNotNull(result);
            Assert.AreEqual("test", result.Message);
        }
    }

    [Serializable]
    public class TestClass {
        public string Message { get; set; }


        public TestClass() {

        }

        public TestClass(string message) {
            Message = message;
        }
    }
}