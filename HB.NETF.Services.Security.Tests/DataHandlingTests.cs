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
        [TestMethod]
        public void TestKeyFileStream() {
            using (FileStream fs = new FileStream("test", FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                KeyStreamHandler keyHandler = new KeyStreamHandler(fs, typeof(AesKey));

                keyHandler.Write(KeyGenerator.GenerateAesKey());
                IKey key = keyHandler.Read();

                Assert.IsNotNull(key);
            }
        }

        [TestMethod]
        public void TestIdentifierFileStream() {
            IIdentifier<TestClass> idWrapper = new IdentifierObject<TestClass>(new TestClass("TestContent", 10));

            using (FileStream fs = new FileStream("test", FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                IdentifierStreamHandler<TestClass> streamHandler = new IdentifierStreamHandler<TestClass>(fs, typeof(IdentifierObject<TestClass>));
                streamHandler.Write(idWrapper);

                IIdentifier<TestClass> idWrapperRead = streamHandler.Read();

                Assert.AreEqual(idWrapper.Reference.Count, idWrapperRead.Reference.Count);
                Assert.AreEqual(idWrapper.Reference.Content, idWrapperRead.Reference.Content);
            }
        }

        [TestMethod]
        public void TestIdentifierFileStreamInvalid() {
            ArgumentException e1 = Assert.ThrowsException<ArgumentException>(() => {
                new IdentifierStreamHandler<TestClass>(typeof(string));
            });

            ArgumentException e2 = Assert.ThrowsException<ArgumentException>(() => {
                new IdentifierStreamHandler<TestClass>(typeof(IIdentifier<string>));
            });

            string message = $"{typeof(string).FullName} does not inherit from {typeof(IIdentifier<TestClass>).FullName}";
            Assert.AreEqual(e1.Message, message);
            Assert.AreEqual(e2.Message, message);
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
