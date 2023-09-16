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
    public interface IAsyncStreamWrapper : IStreamWrapper {
        OperationStatus OperationStatus { get; }

        Task<byte[]> StartOpenFileDialogAsync();
        Task StartSaveFileDialogAsync(byte[] content);

        Task<byte[]> ReadFromFileAsync(string filePath);
        Task WriteToFileAsync(string filePath, byte[] content);

        Task<T> StartOpenFileDialogAsync<T>();
        Task StartSaveFileDialogAsync<T>(T content);

        Task<T> ReadFromFileAsymc<T>(string filePath);
        Task WriteToFileAsync<T>(string filePath, T content);

        Task<byte[]> ReadStreamAsync();
        Task WriteStreamAsync(byte[] content);

        Task<T> ReadStreamAsync<T>();
        Task WriteStreamAsync<T>(T content);

        new IAsyncStreamWrapper WithOptions(Func<IStreamOptionBuilder, IStreamWrapper> optionBuilder);
    }

    public enum OperationStatus {
        NotStarted,
        Running,
        Done,
        Failed
    }
}
