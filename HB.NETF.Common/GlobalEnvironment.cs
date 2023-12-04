using System;
using System.IO;
using System.Text;

namespace HB.NETF.Common {
    public static class GlobalEnvironment {
        public static readonly string BasePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\HBApplicationData\\NETF";
        public static Encoding Encoding { get; set; } = Encoding.UTF8;

        static GlobalEnvironment() {
            Directory.CreateDirectory(BasePath);
        }
    }
}
