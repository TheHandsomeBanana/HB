using HB.NETF.Common.Serialization;
using HB.NETF.Services.Security.Cryptography;
using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HB.Services.Storage.Tests {
    [TestClass]
    public class SerializerTests {
        private const string jsonFile = "testjson";
        private const string xmlFile = "testxml";

        [TestMethod]
        public void JsonTest() {
            SerializerService memoryService = new SerializerService();
            SerializerObject<TestClass> memoryObj = new SerializerObject<TestClass>(new TestClass("test"), SerializerMode.Json);

            memoryService.Write(memoryObj, jsonFile);

            TestClass result = memoryService.Read<TestClass>(jsonFile, SerializerMode.Json).Deserialize();

            Assert.IsNotNull(result);
            Assert.AreEqual("test", result.Message);
        }

        [TestMethod]
        public void XmlTest() {
            SerializerService memoryService = new SerializerService();
            SerializerObject<TestClass> memoryObj = new SerializerObject<TestClass>(new TestClass("test"), SerializerMode.Xml);

            memoryService.Write(memoryObj, xmlFile);

            TestClass result = memoryService.Read<TestClass>(xmlFile, SerializerMode.Xml).Deserialize();

            Assert.IsNotNull(result);
            Assert.AreEqual("test", result.Message);
        }

        [TestMethod]
        public void NonGenericJsonTest() {
            SerializerService memoryService = new SerializerService();
            SerializerObject memoryObj = new SerializerObject(new TestClass("test"), SerializerMode.Json);

            memoryService.Write(memoryObj, jsonFile);

            TestClass result = memoryService.Read(jsonFile, SerializerMode.Json).Deserialize(typeof(TestClass)) as TestClass;

            Assert.IsNotNull(result);
            Assert.AreEqual("test", result.Message);
        }

        [TestMethod]
        public void JsonTestWithCryptoService() {
            SerializerService memoryService = new SerializerService();
            SerializerObject memoryObj = new SerializerObject(new TestClass("test"), SerializerMode.Json);
            IKey key = KeyGenerator.GenerateAesKey();

            memoryObj.Serialize(key);

            memoryService.Write(memoryObj, jsonFile);

            TestClass result = memoryService.Read(jsonFile, SerializerMode.Json).Deserialize(typeof(TestClass), key) as TestClass;

            Assert.IsNotNull(result);
            Assert.AreEqual("test", result.Message);
        }

        [TestMethod]
        public void JsonTestWithCryptoServiceRSA() {
            SerializerService memoryService = new SerializerService();
            SerializerObject<TestClass> memoryObj = new SerializerObject<TestClass>(new TestClass("test"), SerializerMode.Json);
            IKey[] rsaKeys = KeyGenerator.GenerateRsaKeys();

            memoryObj.Serialize(rsaKeys[0]);

            memoryService.Write(memoryObj, jsonFile);

            TestClass result = memoryService.Read<TestClass>(jsonFile, SerializerMode.Json).Deserialize(rsaKeys[1]);

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