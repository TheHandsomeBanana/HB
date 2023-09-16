using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Cryptography.Settings;
using HB.NETF.Services.Security.DataHandling.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.DataHandling {
    public interface IStreamWrapper : IDisposable {
        FileStream Stream { get; }
        byte[] StartOpenFileDialog();
        void StartSaveFileDialog(byte[] content);

        byte[] ReadFromFile(string filePath);
        void WriteToFile(string filePath, byte[] content);

        T StartOpenFileDialog<T>();
        void StartSaveFileDialog<T>(T content);

        T ReadFromFile<T>(string filePath);
        void WriteToFile<T>(string filePath, T content);

        byte[] ReadStream();
        void WriteStream(byte[] content);

        T ReadStream<T>();
        void WriteStream<T>(T content);

        IStreamWrapper WithOptions(Func<IStreamOptionBuilder, IStreamWrapper> optionBuilder);
    }
}
