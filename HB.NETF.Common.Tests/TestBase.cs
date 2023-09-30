﻿using HB.NETF.Common.DependencyInjection;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory;
using HB.NETF.Services.Security.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Common.Tests {
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
