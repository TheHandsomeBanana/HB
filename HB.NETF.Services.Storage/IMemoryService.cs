using HB.NETF.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Storage {
    public interface IMemoryService {
        MemoryObject ReadMemory(string location, SerializerMode serializerMode);
        Task<MemoryObject> ReadMemoryAsync(string location, SerializerMode serializerMode);
        void WriteMemory(MemoryObject memoryObject, string location);
        Task WriteMemoryAsync(MemoryObject memoryObject, string location);

        MemoryObject<TObject> ReadMemory<TObject>(string location, SerializerMode serializerMode);
        Task<MemoryObject<TObject>> ReadMemoryAsync<TObject>(string location, SerializerMode serializerMode);
        void WriteMemory<TObject>(MemoryObject<TObject> memoryObject, string location);
        Task WriteMemoryAsync<TObject>(MemoryObject<TObject> memoryObject, string location);
    }
}
