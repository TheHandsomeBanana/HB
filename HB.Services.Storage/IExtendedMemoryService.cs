using HB.Common.Serialization;
using HB.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Storage {
    public interface IExtendedMemoryService {
        public TObject ReadMemory<TObject>(string location, SerializerMode serializerMode);
        public TObject ReadMemory<TObject>(string location, SerializerMode serializerMode, IKey key);
        public Task<TObject> ReadMemoryAsync<TObject>(string location, SerializerMode serializerMode);
        public Task<TObject> ReadMemoryAsync<TObject>(string location, SerializerMode serializerMode, IKey key);

        public void WriteMemory<TObject>(string location, TObject obj, SerializerMode serializerMode);
        public void WriteMemory<TObject>(string location, TObject obj, SerializerMode serializerMode, IKey key);
        public Task WriteMemoryAsync<TObject>(string location, TObject obj, SerializerMode serializerMode);
        public Task WriteMemoryAsync<TObject>(string location, TObject obj, SerializerMode serializerMode, IKey key);
    }
}