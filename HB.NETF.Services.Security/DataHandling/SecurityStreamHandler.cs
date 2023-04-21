using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.DataHandling {
    public abstract class SecurityStreamHandler {
        protected FileStream Stream { get; set; }
        protected string FilePath { get; }
        protected SecurityStreamMode StreamMode { get; }

        private bool instantDisposal = true;
        public bool InstantDisposal {
            get {
                return instantDisposal;
            }
            set {
                if (value)
                    Stream?.Dispose();

                instantDisposal = value;
            }
        }

        public SecurityStreamHandler() {
            Stream = null;
            StreamMode = SecurityStreamMode.FileDialog;
        }

        public SecurityStreamHandler(FileStream stream) {
            instantDisposal = false;
            FilePath = stream.Name;
            Stream = stream;
            StreamMode = SecurityStreamMode.InputStream;
        }

        public SecurityStreamHandler(string filePath) {
            FilePath = filePath;

            if (!InstantDisposal)
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
