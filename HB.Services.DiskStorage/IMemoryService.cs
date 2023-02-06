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

        public MemoryObject<TObject> ReadMemory<TObject>(string location, SerializerMode serializerMode);
        public Task<MemoryObject<TObject>> ReadMemoryAsync<TObject>(string location, SerializerMode serializerMode);
        public void WriteMemory<TObject>(MemoryObject<TObject> memoryObject, string location);
        public Task WriteMemoryAsync<TObject>(MemoryObject<TObject> memoryObject, string location);
    }
}
