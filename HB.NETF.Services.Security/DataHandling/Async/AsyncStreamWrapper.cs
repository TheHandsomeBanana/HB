using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Cryptography.Settings;
using HB.NETF.Services.Security.DataHandling.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.DataHandling.Async {
    public class AsyncStreamWrapper : StreamWrapper, IAsyncStreamWrapper {
        public AsyncStreamWrapper() {
        }

        public AsyncStreamWrapper(FileStream fs) : base(fs) {
        }

        public OperationStatus OperationStatus => throw new NotImplementedException();

        public Task<T> ReadFromFileAsymc<T>(string filePath) {
            throw new NotImplementedException();
        }

        public Task<byte[]> ReadFromFileAsync(string filePath) {
            throw new NotImplementedException();
        }

        public Task<byte[]> ReadStreamAsync() {
            throw new NotImplementedException();
        }

        public Task<T> ReadStreamAsync<T>() {
            throw new NotImplementedException();
        }

        public Task<byte[]> StartOpenFileDialogAsync() {
            throw new NotImplementedException();
        }

        public Task<T> StartOpenFileDialogAsync<T>() {
            throw new NotImplementedException();
        }

        public Task StartSaveFileDialogAsync(byte[] content) {
            throw new NotImplementedException();
        }

        public Task StartSaveFileDialogAsync<T>(T content) {
            throw new NotImplementedException();
        }

        public Task WriteStreamAsync(byte[] content) {
            throw new NotImplementedException();
        }

        public Task WriteStreamAsync<T>(T content) {
            throw new NotImplementedException();
        }

        public Task WriteToFileAsync(string filePath, byte[] content) {
            throw new NotImplementedException();
        }

        public Task WriteToFileAsync<T>(string filePath, T content) {
            throw new NotImplementedException();
        }

        IAsyncStreamWrapper IAsyncStreamWrapper.WithOptions(Func<IStreamOptionBuilder, IStreamWrapper> optionBuilder) {
            throw new NotImplementedException();
        }
    }
}
