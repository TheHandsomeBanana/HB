using HB.NETF.Common.Serialization.Streams;
using HB.NETF.Common;
using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Identifier;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.DataHandling {
    [Obsolete]
    public class IdentifierStreamHandler<TIdentifier> : SecurityStreamHandler, ISecurityStreamHandler<Identifier<TIdentifier>> {
        private readonly Type identifierType;

        [Obsolete]
        public void Dispose() {
            this.Stream?.Dispose();
        }

        [Obsolete]
        public Identifier<TIdentifier> Read() {
            if (StreamMode == SecurityStreamMode.FileDialog) {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "(*jk)|*jk";

                if (!ofd.ShowDialog().Value)
                    return null;

                Stream = new FileStream(ofd.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }

            string json = GlobalEnvironment.Encoding.GetString(Stream.Read());
            Stream.Position = 0;

            return JsonConvert.DeserializeObject(json, identifierType) as Identifier<TIdentifier>;
        }

        [Obsolete]
        public async Task<Identifier<TIdentifier>> ReadAsync() {
            if (StreamMode == SecurityStreamMode.FileDialog)
                throw new NotSupportedException($"{StreamMode} not supported in async execution.");

            string json = GlobalEnvironment.Encoding.GetString(await Stream.ReadAsync());
            Stream.Position = 0;

            return JsonConvert.DeserializeObject(json, identifierType) as Identifier<TIdentifier>;
        }

        [Obsolete]
        public void Write(Identifier<TIdentifier> item) {
            if (StreamMode == SecurityStreamMode.FileDialog) {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "(*jk)|*jk";
                sfd.FileName = "hbkey";

                if (!sfd.ShowDialog().Value)
                    return;

                Stream = new FileStream(sfd.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }

            byte[] json = GlobalEnvironment.Encoding.GetBytes(JsonConvert.SerializeObject(item));
            Stream.Write(json);
            Stream.Position = 0;
        }

        [Obsolete]
        public async Task WriteAsync(Identifier<TIdentifier> item) {
            if (StreamMode == SecurityStreamMode.FileDialog)
                throw new NotSupportedException($"{StreamMode} not supported in async execution.");

            byte[] json = GlobalEnvironment.Encoding.GetBytes(JsonConvert.SerializeObject(item));
            await Stream.WriteAsync(json);
            Stream.Position = 0;
        }
    }
}
