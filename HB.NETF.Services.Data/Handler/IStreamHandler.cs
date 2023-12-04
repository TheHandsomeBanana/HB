using System;
using System.IO;

namespace HB.NETF.Services.Data.Handler {
    public interface IStreamHandler : IDisposable {
        FileStream Stream { get; }
        byte[] StartOpenFileDialog();
        void StartSaveFileDialog(byte[] content);

        T StartOpenFileDialog<T>();
        void StartSaveFileDialog<T>(T content);

        byte[] ReadFromFile(string filePath);
        void WriteToFile(string filePath, byte[] content);

        T ReadFromFile<T>(string filePath);
        void WriteToFile<T>(string filePath, T content);

        byte[] ReadStream();
        void WriteStream(byte[] content);

        T ReadStream<T>();
        void WriteStream<T>(T content);

        IStreamHandler WithOptions(OptionBuilderFunc optionBuilder);

        byte[] GetResultBytes(string content);
        string GetResultString(byte[] content);
    }
}
