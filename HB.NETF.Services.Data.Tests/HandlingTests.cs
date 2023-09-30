using HB.NETF.Common.Tests;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Security.Cryptography.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HB.NETF.Services.Data.Tests {
    [TestClass]
    public class HandlingTests : TestBase {
        [TestMethod]
        public void StreamHandler_EmptyCtor_FileName() {
            StreamHandler streamHandler = new StreamHandler();
            streamHandler.WriteToFile<TestClass>("test", new TestClass("Test", 10));

            TestClass testClass = streamHandler.ReadFromFile<TestClass>("test");

            Assert.IsNotNull(testClass);
            Assert.AreEqual("Test", testClass.Content);
            Assert.AreEqual(10, testClass.Count);
        }

        [TestMethod]
        public void StreamHandler_EmptyCtor_Dialog() {
            StreamHandler streamHandler = new StreamHandler();
            streamHandler.StartSaveFileDialog<TestClass>(new TestClass("Test", 10));

            TestClass testClass = streamHandler.StartOpenFileDialog<TestClass>();

            Assert.IsNotNull(testClass);
            Assert.AreEqual("Test", testClass.Content);
            Assert.AreEqual(10, testClass.Count);
        }

        [TestMethod]
        public void StreamHandler_WithFileStream() {
            using (FileStream fs = new FileStream("test", FileMode.Create, FileAccess.ReadWrite)) {
                using (StreamHandler streamHandler = new StreamHandler(fs)) {
                    streamHandler.WriteStream<TestClass>(new TestClass("Test", 10));

                    TestClass testClass = streamHandler.ReadStream<TestClass>();

                    Assert.IsNotNull(testClass);
                    Assert.AreEqual("Test", testClass.Content);
                    Assert.AreEqual(10, testClass.Count);
                }
            }
        }

        [TestMethod]
        public void StreamHandler_WithBase64Option() {
            StreamHandler streamHandler = new StreamHandler();

            streamHandler.WithOptions(e => e.UseBase64().Set())
                .WriteToFile<TestClass>("test", new TestClass("Test", 10));

            TestClass testClass = streamHandler.WithOptions(e => e.UseBase64().Set()).ReadFromFile<TestClass>("test");

            Assert.IsNotNull(testClass);
            Assert.AreEqual("Test", testClass.Content);
            Assert.AreEqual(10, testClass.Count);
        }

        [TestMethod]
        public void StreamHandler_WithEncryption() {
            StreamHandler streamHandler = new StreamHandler();
            streamHandler
                .WithOptions(e => e.UseCryptography(EncryptionMode.WindowsDataProtectionAPI).UseBase64().Set())
                .WriteToFile("test", new TestClass("Test", 10));

            TestClass testClass = streamHandler
                .WithOptions(e => e.UseCryptography(EncryptionMode.WindowsDataProtectionAPI).UseBase64().Set())
                .ReadFromFile<TestClass>("test");

            Assert.IsNotNull(testClass);
            Assert.AreEqual("Test", testClass.Content);
            Assert.AreEqual(10, testClass.Count);
        }

        [TestMethod]
        public async Task AsyncStreamHandler_WithEncryption() {
            IAsyncStreamHandler streamHandler = new AsyncStreamHandler();
            await streamHandler
                .WithOptions(e => e.UseCryptography(EncryptionMode.WindowsDataProtectionAPI).UseBase64().SetAsync())
                .WriteToFileAsync("testasync", new TestClass("Test", 10));

            TestClass testClass = await streamHandler
                .WithOptions(e => e.UseCryptography(EncryptionMode.WindowsDataProtectionAPI).UseBase64().SetAsync())
                .ReadFromFileAsync<TestClass>("testasync");

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
