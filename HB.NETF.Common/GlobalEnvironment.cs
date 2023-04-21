using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Common {
    public static class GlobalEnvironment {
        public static readonly string BasePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\HBApplicationData\\NETF";
        public static readonly string CachingService = BasePath + "\\CachingService";
        public static readonly string LoggingService = BasePath + "\\LoggingService";
        public static readonly string SecurityService = BasePath + "\\SecurityService";
        public static Encoding Encoding { get; set; } = Encoding.UTF8;

        static GlobalEnvironment() {
            Directory.CreateDirectory(BasePath);
            Directory.CreateDirectory(CachingService);
            Directory.CreateDirectory(LoggingService);
            Directory.CreateDirectory(SecurityService);
        }
    }
}
