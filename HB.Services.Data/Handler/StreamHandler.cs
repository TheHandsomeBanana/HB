﻿using HB.Common;
using HB.Common.Extensions;
using HB.DependencyInjection.Exceptions;
using HB.DependencyInjection.MS;
using HB.Services.Data.Exceptions;
using HB.Services.Data.Handler.Options;
using HB.Services.Security.Cryptography.Interfaces;
using HB.Services.Security.Cryptography.Keys;
using HB.Services.Security.Cryptography.Settings;
using HB.Services.Security.DataProtection;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HB.Services.Data.Handler
{

    public delegate IStreamHandler OptionBuilderFunc(IStreamOptionBuilder optionBuilder);
    public class StreamHandler : IStreamHandler {
        private readonly IAesCryptoService aesCryptoService;
        private readonly IRsaCryptoService rsaCryptoService;
        private readonly IDataProtectionService dataProtectionService;

        internal StreamOptions Options { get; set; } = new StreamOptions();
        public FileStream? Stream { get; private set; }

        public StreamHandler() {
            aesCryptoService = DIContainer.GetService<IAesCryptoService>() ?? throw new ServiceNotFoundException(nameof(IAesCryptoService));
            rsaCryptoService = DIContainer.GetService<IRsaCryptoService>() ?? throw new ServiceNotFoundException(nameof(IRsaCryptoService));
            dataProtectionService = DIContainer.GetService<IDataProtectionService>() ?? throw new ServiceNotFoundException(nameof(IDataProtectionService));
        }

        public StreamHandler(FileStream fs) : this() {
            Stream = fs;
        }

        #region File
        public byte[] ReadFromFile(string filePath) => Invoke(() => ReadInternal(filePath));

        public T? ReadFromFile<T>(string filePath) {
            return Invoke<T>(() => {
                string content = GlobalEnvironment.Encoding.GetString(ReadInternal(filePath));
                T? tContent = JsonConvert.DeserializeObject<T>(content);
                return tContent;
            });
        }

        public void WriteToFile(string filePath, byte[] content) => Invoke(() => WriteInternal(filePath, content));


        public void WriteToFile<T>(string filePath, T content) {
            Invoke(() => {
                string sContent = JsonConvert.SerializeObject(content, Formatting.Indented);
                byte[] buffer = GlobalEnvironment.Encoding.GetBytes(sContent);
                WriteInternal(filePath, buffer);
            });
        }
        #endregion

        #region Stream
        public byte[] ReadStream() => Invoke(() => ReadStreamInternal());

        public T? ReadStream<T>() {
            return Invoke(() => {
                byte[] buffer = ReadStreamInternal();
                string content = GlobalEnvironment.Encoding.GetString(buffer);
                T? tContent = JsonConvert.DeserializeObject<T>(content);
                return tContent;
            });
        }

        public void WriteStream(byte[] content) => Invoke(() => WriteStreamInternal(content));

        public void WriteStream<T>(T content) {
            Invoke(() => {
                string sContent = JsonConvert.SerializeObject(content, Formatting.Indented);
                byte[] buffer = GlobalEnvironment.Encoding.GetBytes(sContent);
                WriteStreamInternal(buffer);
            });
        }
        #endregion

        #region Read Write Internal
        private byte[] ReadInternal(string fileName) {
            byte[] buffer;
            using (FileStream fs = new FileStream(fileName, FileMode.Open)) {
                buffer = fs.Read();
            }

            if (Options.UseEncryption)
                buffer = DecryptBuffer(buffer);

            if (Options.UseBase64)
                buffer = FromBase64GetString(buffer);

            return buffer;
        }

        private void WriteInternal(string fileName, byte[] buffer) {
            if (Options.UseBase64)
                buffer = GetBytesToBase64(buffer);

            if (Options.UseEncryption)
                buffer = EncryptBuffer(buffer);

            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                fs.Write(buffer);
        }

        private byte[] ReadStreamInternal() {
            if (Stream is null)
                ThrowNoStreamProvided(); 

            byte[] buffer = Stream.Read();

            if(Options.UseEncryption)
                buffer = DecryptBuffer(buffer);

            if (Options.UseBase64)
                buffer = FromBase64GetString(buffer);

            return buffer;
        }

        private void WriteStreamInternal(byte[] buffer) {
            if (Stream == null)
                ThrowNoStreamProvided();

            if (Options.UseBase64)
                buffer = GetBytesToBase64(buffer);

            if (Options.UseEncryption)
                buffer = EncryptBuffer(buffer);

            Stream.Write(buffer);
        }

        #endregion

        public IStreamHandler WithOptions(OptionBuilderFunc optionBuilder) {
            return optionBuilder?.Invoke(new StreamOptionBuilder(this)) ?? this;
        }

        protected virtual void FinishOperation() {
            Options = new StreamOptions();
            Stream?.ResetPosition();
        }
        protected virtual void Invoke(Action action) {

            try {
                action();
            }
            catch (Exception e) {
                throw new StreamHandlerException($"Action failed: {action.Method.Name}.", e);
            }
            finally {
                FinishOperation();
            }
        }
        protected virtual T Invoke<T>(Func<T> func) {
            try {
                return func();
            }
            catch (Exception e) {
                throw new StreamHandlerException($"Action failed: {func.Method.Name}.", e);
            }
            finally {
                FinishOperation();
            }
        }
        protected byte[] FromBase64GetString(byte[] buffer) => Convert.FromBase64String(GlobalEnvironment.Encoding.GetString(buffer));
        protected byte[] GetBytesToBase64(byte[] buffer) => GlobalEnvironment.Encoding.GetBytes(Convert.ToBase64String(buffer));
        protected void ThrowNoStreamProvided() => throw new StreamHandlerException($"No stream provided.");

        public void Dispose() => Stream?.Dispose();

        public byte[] GetResultBytes(string content) => GlobalEnvironment.Encoding.GetBytes(content);
        public string GetResultString(byte[] content) => GlobalEnvironment.Encoding.GetString(content);

        #region Cryptography
        protected byte[] DecryptBuffer(byte[] buffer) {
            switch (Options.EncryptionMode) {
                case EncryptionMode.WindowsDataProtectionAPI:
                    return dataProtectionService.Unprotect(buffer);
                case EncryptionMode.AES:
                    if (Options.Key == null)
                        throw new StreamHandlerException($"No key for aes decryption provided.");

                    return aesCryptoService.Decrypt(buffer, Options.Key);
                case EncryptionMode.RSA:
                    if (Options.Key == null)
                        throw new StreamHandlerException($"No key for rsa decryption provided.");

                    return rsaCryptoService.Decrypt(buffer, Options.Key);
            }

            return buffer;
        }

        protected byte[] EncryptBuffer(byte[] buffer) {
            switch (Options.EncryptionMode) {
                case EncryptionMode.WindowsDataProtectionAPI:
                    return dataProtectionService.Protect(buffer);
                case EncryptionMode.AES:
                    if (Options.Key == null)
                        throw new StreamHandlerException($"No key for aes enryption provided.");

                    return aesCryptoService.Encrypt(buffer, Options.Key);
                case EncryptionMode.RSA:
                    if (Options.Key == null)
                        throw new StreamHandlerException($"No key for rsa encryption provided.");

                    return rsaCryptoService.Encrypt(buffer, Options.Key);
            }

            return buffer;
        }
        #endregion
    }
}

