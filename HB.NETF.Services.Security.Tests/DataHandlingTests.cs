using HB.NETF.Services.Security.Cryptography;
using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.DataHandling;
using HB.NETF.Services.Security.Identifier;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Tests {
    [TestClass]
    public class DataHandlingTests {
        [Obsolete]
        //[TestMethod]
        public void TestKeyFileStream() {
            using (FileStream fs = new FileStream("test", FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                KeyStreamHandler keyHandler = new KeyStreamHandler(fs, typeof(AesKey));

                keyHandler.Write(KeyGenerator.GenerateAesKey());
                IKey key = keyHandler.Read();

                Assert.IsNotNull(key);
            }
        }

        [TestMethod]
        public void StreamWrapper_EmptyCtor_FileName() {
            StreamWrapper streamWrapper = new StreamWrapper();
            streamWrapper.WriteToFile<TestClass>("test", new TestClass("Test", 10));

            TestClass testClass = streamWrapper.ReadFromFile<TestClass>("test");

            Assert.IsNotNull(testClass);
            Assert.AreEqual("Test", testClass.Content);
            Assert.AreEqual(10, testClass.Count);
        }

        [TestMethod]
        public void StreamWrapper_EmptyCtor_Dialog() {
            StreamWrapper streamWrapper = new StreamWrapper();
            streamWrapper.StartSaveFileDialog<TestClass>(new TestClass("Test", 10));

            TestClass testClass = streamWrapper.StartOpenFileDialog<TestClass>();

            Assert.IsNotNull(testClass);
            Assert.AreEqual("Test", testClass.Content);
            Assert.AreEqual(10, testClass.Count);
        }

        [TestMethod]
        public void StreamWrapper_WithFileStream() {
            using(FileStream fs = new FileStream("test", FileMode.Create, FileAccess.ReadWrite)) {
                using(StreamWrapper streamWrapper = new StreamWrapper(fs)) {
                    streamWrapper.WriteStream<TestClass>(new TestClass("Test", 10));

                    TestClass testClass = streamWrapper.ReadStream<TestClass>();

                    Assert.IsNotNull(testClass);
                    Assert.AreEqual("Test", testClass.Content);
                    Assert.AreEqual(10, testClass.Count);
                }
            }
        }

        [TestMethod]
        public void StreamWrapper_WithBase64Option() {
            StreamWrapper streamWrapper = new StreamWrapper();

            streamWrapper.WithOptions(e => e.UseBase64().Set())
                .WriteToFile<TestClass>("test", new TestClass("Test", 10));

            TestClass testClass = streamWrapper.WithOptions(e => e.UseBase64().Set()).ReadFromFile<TestClass>("test");

            Assert.IsNotNull(testClass);
            Assert.AreEqual("Test", testClass.Content);
            Assert.AreEqual(10, testClass.Count);
        }

        
    }

    public class TestClass {
        public string Content { get; set; }
        public int Count { get; set; }

        public TestClass(string content, int count) {
            Content = content;
            Count = count;
        }
    }
}
