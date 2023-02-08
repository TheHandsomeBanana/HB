using HB.Services.Logging.Factory;
using HB.Services.Logging.Factory.Builder;
using HB.Services.Logging.Factory.Target;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;
using HB.Services.Logging;

namespace HB.Services.Logging.Tests {
    [TestClass]
    public class LoggingTests {
        [TestMethod]
        public void TestFile() {
            const string file = "logfile";

            ILoggerFactory factory = new LoggerFactory();
            ILogger logger = factory.CreateLogger<LoggingTests>(b => b.AddTarget(file));

            logger.LogInformation("Testmessage");
        }

        [TestMethod]
        public void TestStream() {
            MemoryStream fs = new MemoryStream();

            ILoggerFactory factory = new LoggerFactory();
            ILogger logger = factory.CreateLogger<LoggingTests>(b => b.AddTarget(fs));

            logger.LogInformation("Testmessage");

            using (StreamReader sr = new StreamReader(fs)) {
                Console.WriteLine(sr.ReadToEnd());
            }
        }

        [TestMethod]
        public void TestAction() {
            ILoggerFactory factory = new LoggerFactory();
            ILogger logger = factory.CreateLogger<LoggingTests>(b => b.AddTargets(OnLog, OnLog2));

            logger.LogInformation("Testmessage");
        }

        private void OnLog(LogStatement logStatement) {
            Console.WriteLine(logStatement.ToString());
        }

        private void OnLog2(string log) {
            Console.WriteLine(log);
        }
    }
}