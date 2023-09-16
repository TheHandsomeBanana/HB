using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.DataHandling {
    [Obsolete]
    public abstract class SecurityStreamHandler {
        protected FileStream Stream { get; set; }
        protected string FilePath { get; }
        protected SecurityStreamMode StreamMode { get; }

        [Obsolete]
        public SecurityStreamHandler() {
            StreamMode = SecurityStreamMode.FileDialog;
        }

        [Obsolete]
        public SecurityStreamHandler(FileStream stream) {
            FilePath = stream.Name;
            Stream = stream;
            StreamMode = SecurityStreamMode.InputStream;
        }

        [Obsolete]
        public SecurityStreamHandler(string filePath) {
            FilePath = filePath;
            Stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamMode = SecurityStreamMode.FilePath;
        }

        protected enum SecurityStreamMode {
            InputStream,
            FilePath,
            FileDialog
        }
    }


}
