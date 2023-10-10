using HB.NETF.Services.Logging.Factory;
using HB.NETF.Services.Logging.Factory.Builder;
using HB.NETF.Services.Logging.Factory.Target;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;
using HB.NETF.Services.Logging;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using HB.NETF.Services.Logging.Exceptions;
using HB.NETF.Common.Exceptions;

namespace HB.NETF.Services.Logging.Tests {
    [TestClass]
    public class LoggingTests {
        [TestMethod]
        public void TestFile() {
            const string file = "logfile";

            ILoggerFactory factory = new LoggerFactory();
            ILogger logger = factory.CreateLogger<LoggingTests>(b => b.AddTarget(file));

            logger.LogInformation("Testmessage");

            Assert.IsTrue(File.Exists(file));
            Assert.AreEqual("Testmessage", File.ReadAllText(file));
        }

        [TestMethod]
        public void TestStream() {
            MemoryStream fs = new MemoryStream();

            ILoggerFactory factory = new LoggerFactory();
            ILogger logger = factory.CreateLogger<LoggingTests>(b => b.AddTarget(fs));

            logger.LogInformation("Testmessage");

            using (StreamReader sr = new StreamReader(fs)) {
                Assert.AreEqual("Testmessage", sr.ReadToEnd());
            }
        }

        [TestMethod]
        public void TestAction() {
            ILoggerFactory factory = new LoggerFactory();
            ILogger logger = factory.CreateLogger<LoggingTests>(b => b.AddTarget(OnLog).AddTarget(OnLog2));

            logger.LogInformation("Testmessage");
        }

        private void OnLog(LogStatement logStatement) {
            Console.WriteLine(logStatement.ToString());
        }

        private void OnLog2(string log) {
            Console.WriteLine(log);
        }

        [TestMethod]
        public void CreateExistingLogger_ThrowsException() {
            ILoggerFactory factory = new LoggerFactory();
            factory.CreateLogger<LoggingTests>(b => b.WithNoTargets());
            Assert.ThrowsException<LoggerException>(() => factory.CreateLogger<LoggingTests>(b => b.WithNoTargets()));
        }
    }
}