using HB.Services.Data.Handler.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Data.Handler.Async {
    public interface IAsyncStreamHandler : IStreamHandler {
        [SupportedOSPlatform("windows")]
        Task<byte[]> StartOpenFileDialogAsync();
        [SupportedOSPlatform("windows")]
        Task StartSaveFileDialogAsync(byte[] content);

        [SupportedOSPlatform("windows")]
        Task<T?> StartOpenFileDialogAsync<T>();
        [SupportedOSPlatform("windows")]
        Task StartSaveFileDialogAsync<T>(T content);

        Task<byte[]> ReadFromFileAsync(string filePath);
        Task<T?> ReadFromFileAsync<T>(string filePath);

        Task WriteToFileAsync(string filePath, byte[] content);
        Task WriteToFileAsync<T>(string filePath, T content);

        Task<byte[]> ReadStreamAsync();
        Task<T?> ReadStreamAsync<T>();

        Task WriteStreamAsync(byte[] content);
        Task WriteStreamAsync<T>(T content);

        new IAsyncStreamHandler WithOptions(OptionBuilderFunc optionBuilder);
    }
}
