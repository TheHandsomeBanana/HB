using HB.NETF.Services.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HB.Services.Serialization.Tests {
    [TestClass]
    public class SerializerTests {
        private const string jsonFile = "testjson";
        private const string xmlFile = "testxml";

        [TestMethod]
        public void JsonTest() {
            SerializerService serializer = new SerializerService();
            
        }

        [TestMethod]
        public void XmlTest() {
            SerializerService serializer = new SerializerService();

        }

        [TestMethod]
        public void NonGenericJsonTest() {
            SerializerService serializer = new SerializerService();

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