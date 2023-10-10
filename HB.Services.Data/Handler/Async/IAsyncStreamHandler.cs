using HB.Services.Data.Handler.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Data.Handler.Async {
    public interface IAsyncStreamHandler : IStreamHandler {

        Task<byte[]> ReadFromFileAsync(string filePath);
        Task WriteToFileAsync(string filePath, byte[] content);

        Task<T?> ReadFromFileAsync<T>(string filePath);
        Task WriteToFileAsync<T>(string filePath, T content);

        Task<byte[]> ReadStreamAsync();
        Task WriteStreamAsync(byte[] content);

        Task<T?> ReadStreamAsync<T>();
        Task WriteStreamAsync<T>(T content);

        new IAsyncStreamHandler WithOptions(OptionBuilderFunc optionBuilder);
    }
}
