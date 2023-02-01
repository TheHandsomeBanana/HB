using HB.Common;
using HB.Services.Security.Cryptography.Keys;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Security.Cryptography;
using HB.Common.Serialization.Streams;

namespace HB.Services.Security.DataHandling
{
    public class KeyStreamHandler : SecurityStreamHandler, ISecurityStreamHandler<IKey> {
        private Type keyType;

        public KeyStreamHandler(Type keyType) : base() {
            this.keyType = keyType;
        }

        public KeyStreamHandler(FileStream stream, Type keyType) : base(stream) {
            this.keyType = keyType;
        }

        public KeyStreamHandler(string filePath, Type keyType) : base(filePath) {
            this.keyType = keyType;
        }

        public IKey? Read() {
            //if(StreamMode == SecurityStreamMode.FileDialog) {
            //    OpenFileDialog ofd = new OpenFileDialog();
            //    ofd.Filter = "(*jk)|*jk";

            //    if (ofd.ShowDialog() == true) {
            //        Stream = new FileStream(ofd.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //    }
            //}

            if (StreamMode == SecurityStreamMode.FileDialog)
                throw new NotSupportedException($"{StreamMode} is currently not working.");

            string? json;
            if (InstantDisposal) {
                Stream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                json = GlobalEnvironment.Encoding.GetString(Stream.Read());
                Stream.Dispose();
            }
            else {
                json = GlobalEnvironment.Encoding.GetString(Stream.Read());
                Stream.Position = 0;
            }

            return JsonConvert.DeserializeObject(json, keyType) as IKey;
        }

        public async Task<IKey?> ReadAsync() {
            if (StreamMode == SecurityStreamMode.FileDialog)
                throw new NotSupportedException($"{StreamMode} not supported in async execution.");

            string? json;
            if (InstantDisposal) {
                Stream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                json = GlobalEnvironment.Encoding.GetString(await Stream.ReadAsync());
                await Stream.DisposeAsync();
            }
            else {
                json = GlobalEnvironment.Encoding.GetString(await Stream.ReadAsync());
                Stream.Position = 0;
            }

            return JsonConvert.DeserializeObject(json, keyType) as IKey;
        }

        public void Write(IKey item) {
            //if (StreamMode == SecurityStreamMode.FileDialog) {
            //    SaveFileDialog sfd = new SaveFileDialog();
            //    sfd.Filter = "(*jk)|*jk";

            //    if (sfd.ShowDialog() == true) {
            //        Stream = new FileStream(sfd.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //    }
            //}

            if (StreamMode == SecurityStreamMode.FileDialog)
                throw new NotSupportedException($"{StreamMode} is currently not working.");

            byte[] json = GlobalEnvironment.Encoding.GetBytes(JsonConvert.SerializeObject(item));

            if (InstantDisposal) {
                Stream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                Stream.Write(json);
                Stream.Dispose();
            }
            else {
                Stream.Write(json);
                Stream.Position = 0;
            }

        }

        public async Task WriteAsync(IKey item) {
            if (StreamMode == SecurityStreamMode.FileDialog)
                throw new NotSupportedException($"{StreamMode} not supported in async execution.");

            byte[] json = GlobalEnvironment.Encoding.GetBytes(JsonConvert.SerializeObject(item));

            if (InstantDisposal) {
                Stream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                await Stream.WriteAsync(json);
                await Stream.DisposeAsync();
            }
            else {
                await Stream.WriteAsync(json);
                Stream.Position = 0;
            }
        }

        public void Dispose() {
            this.Stream?.Dispose();
        }
    }
}
