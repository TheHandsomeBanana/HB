using HB.Services.DiskStorage.Objects;

namespace HB.Services.DiskStorage {
    public class MemoryService : IMemoryService {
        public MemoryObject ReadMemory(string location) {
            throw new NotImplementedException();
        }

        public MemoryObject<TMemoryObject> ReadMemory<TMemoryObject>(string location) {
            throw new NotImplementedException();
        }

        public Task<MemoryObject> ReadMemoryAsync(string location) {
            throw new NotImplementedException();
        }

        public Task<MemoryObject<TMemoryObject>> ReadMemoryAsync<TMemoryObject>(string location) {
            throw new NotImplementedException();
        }

        public void WriteMemory(MemoryObject memoryObject, string location) {
            throw new NotImplementedException();
        }

        public void WriteMemory<TMemoryObject>(MemoryObject<TMemoryObject> memoryObject, string location) {
            throw new NotImplementedException();
        }

        public Task WriteMemoryAsync(MemoryObject memoryObject, string location) {
            throw new NotImplementedException();
        }

        public Task WriteMemoryAsync<TMemoryObject>(MemoryObject<TMemoryObject> memoryObject, string location) {
            throw new NotImplementedException();
        }
    }
}