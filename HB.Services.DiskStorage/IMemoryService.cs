using HB.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.DiskStorage
{
    public interface IMemoryService {
        public MemoryObject ReadMemory(string location, SerializerMode serializerMode);
        public Task<MemoryObject> ReadMemoryAsync(string location, SerializerMode serializerMode);
        public void WriteMemory(MemoryObject memoryObject, string location);
        public Task WriteMemoryAsync(MemoryObject memoryObject, string location);

        public MemoryObject<TMemoryObject> ReadMemory<TMemoryObject>(string location, SerializerMode serializerMode);
        public Task<MemoryObject<TMemoryObject>> ReadMemoryAsync<TMemoryObject>(string location, SerializerMode serializerMode);
        public void WriteMemory<TMemoryObject>(MemoryObject<TMemoryObject> memoryObject, string location);
        public Task WriteMemoryAsync<TMemoryObject>(MemoryObject<TMemoryObject> memoryObject, string location);
    }
}
