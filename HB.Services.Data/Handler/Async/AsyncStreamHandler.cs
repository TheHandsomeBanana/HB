﻿using HB.Services.Data.Handler;
using HB.Common.Extensions;
using HB.Services.Data.Handler.Async;
using HB.Services.Data.Handler.Options;
using HB.Services.Security.Cryptography.Keys;
using HB.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HB.Common;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Runtime.Versioning;

namespace HB.Services.Data.Handler.Async {
    public class AsyncStreamHandler : StreamHandler, IAsyncStreamHandler {
        public AsyncStreamHandler() : base() {
        }

        public AsyncStreamHandler(FileStream fs) : base(fs) {
        }

        #region File

        public async Task<byte[]> ReadFromFileAsync(string filePath) => await Invoke(async () => await ReadInternal(filePath));
        public async Task<T?> ReadFromFileAsync<T>(string filePath) {
            return await Invoke(async () => {
                string content = GlobalEnvironment.Encoding.GetString(await ReadInternal(filePath));
                T? tContent = JsonConvert.DeserializeObject<T>(content);
                return tContent;
            });
        }

        public async Task WriteToFileAsync(string filePath, byte[] content) => await Invoke(async () => await WriteInternal(filePath, content));

        public async Task WriteToFileAsync<T>(string filePath, T content) {
            await Invoke(async () => {

                string sContent = JsonConvert.SerializeObject(content);
                byte[] buffer = GlobalEnvironment.Encoding.GetBytes(sContent);
                await WriteInternal(filePath, buffer);
            });
        }
        #endregion

        #region Stream
        public async Task<byte[]> ReadStreamAsync() => await Invoke(async () => await ReadStreamInternal()); 

        public async Task<T?> ReadStreamAsync<T>() {
            return await Invoke(async () => {
                byte[] buffer = await ReadStreamInternal();
                string content = GlobalEnvironment.Encoding.GetString(buffer);
                T? tContent = JsonConvert.DeserializeObject<T>(content);
                return tContent;
            });
        }

        public async Task WriteStreamAsync(byte[] content) => await Invoke(async () => await WriteStreamInternal(content));

        public async Task WriteStreamAsync<T>(T content) {
            await Invoke(async () => {
                string sContent = JsonConvert.SerializeObject(content);
                byte[] buffer = GlobalEnvironment.Encoding.GetBytes(sContent);
                await WriteStreamInternal(buffer);
            });
        }
        #endregion

        #region Read Write Internal
        private async Task<byte[]> ReadInternal(string fileName) {
            byte[] buffer;
            using (FileStream fs = new FileStream(fileName, FileMode.Open)) {
                buffer = await fs.ReadAsync();
            }

            if (Options.UseEncryption)
                buffer = DecryptBuffer(buffer);

            if (Options.UseBase64)
                buffer = FromBase64GetString(buffer);

            return buffer;
        }
        private async Task WriteInternal(string fileName, byte[] buffer) {
            if (Options.UseBase64)
                buffer = GetBytesToBase64(buffer);

            if (Options.UseEncryption)
                buffer = EncryptBuffer(buffer);


            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                await fs.WriteAsync(buffer);
        }
        private async Task<byte[]> ReadStreamInternal() {
            if (Stream == null)
                ThrowNoStreamProvided();

            byte[] buffer = await Stream.ReadAsync();

            if (Options.UseEncryption)
                buffer = DecryptBuffer(buffer);

            if (Options.UseBase64)
                buffer = FromBase64GetString(buffer);

            return buffer;
        }
        private async Task WriteStreamInternal(byte[] buffer) {
            if (Stream == null)
                ThrowNoStreamProvided();

            if (Options.UseBase64)
                buffer = GetBytesToBase64(buffer);

            if (Options.UseEncryption)
                buffer = EncryptBuffer(buffer);

            await Stream.WriteAsync(buffer);
        }
        #endregion

        IAsyncStreamHandler IAsyncStreamHandler.WithOptions(OptionBuilderFunc optionBuilder) => (IAsyncStreamHandler)this.WithOptions(optionBuilder);
        protected override void FinishOperation() {
            Options = new StreamOptions();
            Stream?.ResetPosition();
        }
    }
}
