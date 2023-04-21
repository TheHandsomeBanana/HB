using HB.NETF.Common.Serialization;
using HB.NETF.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Storage {
    public interface IExtendedMemoryService {
        TObject ReadMemory<TObject>(string location, SerializerMode serializerMode);
        TObject ReadMemory<TObject>(string location, SerializerMode serializerMode, IKey key);
        Task<TObject> ReadMemoryAsync<TObject>(string location, SerializerMode serializerMode);
        Task<TObject> ReadMemoryAsync<TObject>(string location, SerializerMode serializerMode, IKey key);

        void WriteMemory<TObject>(string location, TObject obj, SerializerMode serializerMode);
        void WriteMemory<TObject>(string location, TObject obj, SerializerMode serializerMode, IKey key);
        Task WriteMemoryAsync<TObject>(string location, TObject obj, SerializerMode serializerMode);
        Task WriteMemoryAsync<TObject>(string location, TObject obj, SerializerMode serializerMode, IKey key);
    }
}