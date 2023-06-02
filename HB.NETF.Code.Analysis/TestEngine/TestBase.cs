using HB.NETF.Common.DependencyInjection;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.TestEngine {
    [TestClass]
    public abstract class TestBase {
        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassInitialize(TestContext context) {
            ILoggerFactory loggerFactory = new LoggerFactory();
            
            DIBuilder builder = new DIBuilder();
            builder.Services.AddSingleton(loggerFactory.CreateLogger<TestBase>(b => b.AddTarget(OnLog)));

            DIContainer.BuildServiceProvider(builder);
        }

        private static void OnLog(LogStatement obj) {
            Console.WriteLine(obj.ToString());
        }
    }
}