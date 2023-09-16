using HB.NETF.Common;
using HB.NETF.Common.Serialization.Streams;
using HB.NETF.Services.Security.DataHandling.Options;
using HB.NETF.Services.Security.Exceptions;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Resources;

namespace HB.NETF.Services.Security.DataHandling {
    public class StreamWrapper : IStreamWrapper {
        internal StreamOptions Options { get; private set; } = new StreamOptions();
        public FileStream Stream { get; private set; }

        public StreamWrapper() {

        }

        public StreamWrapper(FileStream fs) {
            Stream = fs;
        }

        #region File
        public byte[] ReadFromFile(string filePath) {
            return Invoke<byte[]>(() => {
                byte[] buffer = ReadInternal(filePath);
                return buffer;
            });
        }

        public T ReadFromFile<T>(string filePath) {
            return Invoke<T>(() => {
                string content = GlobalEnvironment.Encoding.GetString(ReadInternal(filePath));
                T tContent = JsonConvert.DeserializeObject<T>(content);
                return tContent;
            });
        }

        public void WriteToFile(string filePath, byte[] content) => Invoke(() => WriteInternal(filePath, content));


        public void WriteToFile<T>(string filePath, T content) {
            Invoke(() => {

                string sContent = JsonConvert.SerializeObject(content);
                byte[] buffer = GlobalEnvironment.Encoding.GetBytes(sContent);
                WriteInternal(filePath, buffer);
            });
        }
        #endregion

        #region Dialog
        public byte[] StartOpenFileDialog() => Invoke(() => ReadFromDialog() ?? Array.Empty<byte>());

        public T StartOpenFileDialog<T>() {
            return Invoke(() => {
                byte[] buffer = ReadFromDialog() ?? Array.Empty<byte>();

                string sContent = GlobalEnvironment.Encoding.GetString(buffer);
                T content = JsonConvert.DeserializeObject<T>(sContent);
                return content;
            });
        }

        public void StartSaveFileDialog(byte[] content) {
            Invoke(() => {
                WriteDialog(content);
            });
        }

        public void StartSaveFileDialog<T>(T content) {
            Invoke(() => {
                string sContent = JsonConvert.SerializeObject(content);
                byte[] buffer = GlobalEnvironment.Encoding.GetBytes(sContent);

                WriteDialog(buffer);
            });
        }

        private byte[] ReadFromDialog() {
            OpenFileDialog ofd = new OpenFileDialog();
            if (!ofd.ShowDialog().Value)
                return null;

            return ReadInternal(ofd.FileName);
        }
        private void WriteDialog(byte[] buffer) {
            SaveFileDialog sfd = new SaveFileDialog();

            if (!sfd.ShowDialog().Value)
                return;

            WriteInternal(sfd.FileName, buffer);
        }
        #endregion

        #region Stream
        public byte[] ReadStream() => Invoke(() => ReadStreamInternal());

        public T ReadStream<T>() {
            return Invoke(() => {
                byte[] buffer = ReadStreamInternal();
                string content = GlobalEnvironment.Encoding.GetString(buffer);
                T tContent = JsonConvert.DeserializeObject<T>(content);
                return tContent;
            });
        }

        public void WriteStream(byte[] content) => Invoke(() => WriteStreamInternal(content));

        public void WriteStream<T>(T content) {
            Invoke(() => {
                string sContent = JsonConvert.SerializeObject(content);
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

            if (Options.UseBase64)
                buffer = FromBase64GetString(buffer);

            return buffer;
        }

        private void WriteInternal(string fileName, byte[] buffer) {
            if (Options.UseBase64)
                buffer = GetBytesToBase64(buffer);


            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                fs.Write(buffer);
        }

        private byte[] ReadStreamInternal() {
            if (Stream == null)
                ThrowNoStreamProvided();

            byte[] buffer = Stream.Read();

            if (Options.UseBase64)
                buffer = FromBase64GetString(buffer);

            return buffer;
        }

        private void WriteStreamInternal(byte[] buffer) {
            if (Stream == null)
                ThrowNoStreamProvided();

            if (Options.UseBase64)
                buffer = GetBytesToBase64(buffer);

            Stream.Write(buffer);
        }

        protected virtual void FinishOperation() {
            Options = new StreamOptions();
            Stream?.ResetPosition();
        }
        #endregion

        public IStreamWrapper WithOptions(Func<IStreamOptionBuilder, IStreamWrapper> optionBuilder) {
            return optionBuilder?.Invoke(new StreamOptionBuilder(this));
        }

        protected virtual void Invoke(Action action) {

            try {
                action();
            }
            catch (Exception e) {
                throw new StreamWrapperException($"Action failed: {action.Method.Name}.", e);
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
                throw new StreamWrapperException($"Action failed: {func.Method.Name}.", e);
            }
            finally {
                FinishOperation();
            }
        }
        private byte[] FromBase64GetString(byte[] buffer) => Convert.FromBase64String(GlobalEnvironment.Encoding.GetString(buffer));
        private byte[] GetBytesToBase64(byte[] buffer) => GlobalEnvironment.Encoding.GetBytes(Convert.ToBase64String(buffer));
        private void ThrowNoStreamProvided() => throw new StreamWrapperException($"No stream provided.");

        public void Dispose() {
            Stream?.Dispose();
        }
    }
}
