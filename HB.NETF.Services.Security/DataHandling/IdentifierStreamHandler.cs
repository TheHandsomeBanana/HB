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
    public class IdentifierStreamHandler<TIdentifier> : SecurityStreamHandler, ISecurityStreamHandler<IIdentifier<TIdentifier>> {
        private readonly Type identifierType;

        public IdentifierStreamHandler(Type identifierType) {
            if (!identifierType.GetInterfaces().Contains(typeof(IIdentifier<TIdentifier>)))
                throw new ArgumentException($"{identifierType.FullName} does not inherit from {typeof(IIdentifier<TIdentifier>).FullName}");

            this.identifierType = identifierType;
       }

        public IdentifierStreamHandler(FileStream stream, Type identifierType) : base(stream) {
            if (!identifierType.GetInterfaces().Contains(typeof(IIdentifier<TIdentifier>)))
                throw new ArgumentException($"{identifierType.FullName} does not inherit from {typeof(IIdentifier<TIdentifier>).FullName}");
            
            this.identifierType = identifierType;
        }

        public IdentifierStreamHandler(string filePath, Type identifierType) : base(filePath) {
            if (!identifierType.GetInterfaces().Contains(typeof(IIdentifier<TIdentifier>)))
                throw new ArgumentException($"{identifierType.FullName} does not inherit from {typeof(IIdentifier<TIdentifier>).FullName}");

            this.identifierType = identifierType;
        }

        public void Dispose() {
            this.Stream?.Dispose();
        }

        public IIdentifier<TIdentifier> Read() {
            if (StreamMode == SecurityStreamMode.FileDialog) {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "(*jk)|*jk";

                if (!ofd.ShowDialog().Value)
                    return null;

                Stream = new FileStream(ofd.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }

            string json = GlobalEnvironment.Encoding.GetString(Stream.Read());
            Stream.Position = 0;

            return JsonConvert.DeserializeObject(json, identifierType) as IIdentifier<TIdentifier>;
        }

        public async Task<IIdentifier<TIdentifier>> ReadAsync() {
            if (StreamMode == SecurityStreamMode.FileDialog)
                throw new NotSupportedException($"{StreamMode} not supported in async execution.");

            string json = GlobalEnvironment.Encoding.GetString(await Stream.ReadAsync());
            Stream.Position = 0;

            return JsonConvert.DeserializeObject(json, identifierType) as IIdentifier<TIdentifier>;
        }

        public void Write(IIdentifier<TIdentifier> item) {
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

        public async Task WriteAsync(IIdentifier<TIdentifier> item) {
            if (StreamMode == SecurityStreamMode.FileDialog)
                throw new NotSupportedException($"{StreamMode} not supported in async execution.");

            byte[] json = GlobalEnvironment.Encoding.GetBytes(JsonConvert.SerializeObject(item));
            await Stream.WriteAsync(json);
            Stream.Position = 0;
        }
    }
}
