using HB.NETF.Services.Security.Cryptography;
using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.DataHandling;
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
    }
}
