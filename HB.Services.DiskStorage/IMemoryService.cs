using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.DiskStorage
{
    public interface IMemoryService {
        public MemoryObject ReadMemory(string location);
        public Task<MemoryObject> ReadMemoryAsync(string location);
        public void WriteMemory(MemoryObject memoryObject, string location);
        public Task WriteMemoryAsync(MemoryObject memoryObject, string location);

        public MemoryObject<TMemoryObject> ReadMemory<TMemoryObject>(string location);
        public Task<MemoryObject<TMemoryObject>> ReadMemoryAsync<TMemoryObject>(string location);
        public void WriteMemory<TMemoryObject>(MemoryObject<TMemoryObject> memoryObject, string location);
        public Task WriteMemoryAsync<TMemoryObject>(MemoryObject<TMemoryObject> memoryObject, string location);
    }
}
