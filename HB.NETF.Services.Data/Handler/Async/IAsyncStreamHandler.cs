using HB.NETF.Services.Data.Handler.Options;
using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Data.Handler.Async {
    public interface IAsyncStreamHandler : IStreamHandler {
        Task<byte[]> StartOpenFileDialogAsync();
        Task StartSaveFileDialogAsync(byte[] content);

        Task<byte[]> ReadFromFileAsync(string filePath);
        Task WriteToFileAsync(string filePath, byte[] content);

        Task<T> StartOpenFileDialogAsync<T>();
        Task StartSaveFileDialogAsync<T>(T content);

        Task<T> ReadFromFileAsync<T>(string filePath);
        Task WriteToFileAsync<T>(string filePath, T content);

        Task<byte[]> ReadStreamAsync();
        Task WriteStreamAsync(byte[] content);

        Task<T> ReadStreamAsync<T>();
        Task WriteStreamAsync<T>(T content);

        new IAsyncStreamHandler WithOptions(OptionBuilderFunc optionBuilder);
    }
}
