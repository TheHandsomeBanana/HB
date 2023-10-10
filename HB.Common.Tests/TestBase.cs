using HB.DependencyInjection;
using HB.Services.Data.Handler;
using HB.Services.Logging;
using HB.Services.Logging.Factory;
using HB.Services.Security.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Common.Tests {
    [TestClass]
    public abstract class TestBase {
        [TestInitialize]
        public void Init() {
            DIBuilder dIBuilder = new DIBuilder();
            dIBuilder.Services.AddSingleton<ILoggerFactory>(new LoggerFactory((b) => b.AddTarget(Output)));
            dIBuilder.Services.AddTransient<IStreamHandler, StreamHandler>();
            dIBuilder.Services.AddTransient<IDataProtectionService, DataProtectionService>();
            DIContainer.BuildServiceProvider(dIBuilder);
        }

        public void Output(LogStatement log) {
            Console.WriteLine(log.ToString());
        }
    }
}
