using HB.NETF.Services.Data.Handler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace HB.NETF.Services.Data.Tests {
    [TestClass]
    public class HandlingTests {
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
