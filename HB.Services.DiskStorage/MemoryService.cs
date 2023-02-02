using HB.Common.Serialization;
using HB.Common.Serialization.Streams;

namespace HB.Services.DiskStorage {
    public class MemoryService : IMemoryService {
        public MemoryObject ReadMemory(string location, SerializerMode serializerMode) {
            string content;
            using (StreamReader sr = new StreamReader(location))
                content = sr.ReadToEnd();

            return new MemoryObject(content, serializerMode);
        }

        public MemoryObject<TMemoryObject> ReadMemory<TMemoryObject>(string location, SerializerMode serializerMode) {
            string content;
            using (StreamReader sr = new StreamReader(location))
                content = sr.ReadToEnd();

            return new MemoryObject<TMemoryObject>(content, serializerMode);
        }

        public async Task<MemoryObject> ReadMemoryAsync(string location, SerializerMode serializerMode) {
            string content;
            using (StreamReader sr = new StreamReader(location))
                content = await sr.ReadToEndAsync();

            return new MemoryObject(content, serializerMode);
        }

        public async Task<MemoryObject<TMemoryObject>> ReadMemoryAsync<TMemoryObject>(string location, SerializerMode serializerMode) {
            string content;
            using (StreamReader sr = new StreamReader(location))
                content = await sr.ReadToEndAsync();

            return new MemoryObject<TMemoryObject>(content, serializerMode);
        }

        public void WriteMemory(MemoryObject memoryObject, string location) {
            if (!memoryObject.IsSerialized)
                memoryObject.Serialize();

            using(StreamWriter sw = new StreamWriter(location)) {
                sw.Write(memoryObject.SerializedObj);
            }
        }

        public void WriteMemory<TMemoryObject>(MemoryObject<TMemoryObject> memoryObject, string location) {
            if (!memoryObject.IsSerialized)
                memoryObject.Serialize();

            using (StreamWriter sw = new StreamWriter(location)) {
                sw.Write(memoryObject.SerializedObj);
            }
        }

        public async Task WriteMemoryAsync(MemoryObject memoryObject, string location) {
            if (!memoryObject.IsSerialized)
                memoryObject.Serialize();

            using (StreamWriter sw = new StreamWriter(location)) {
                await sw.WriteAsync(memoryObject.SerializedObj);
            }
        }

        public async Task WriteMemoryAsync<TMemoryObject>(MemoryObject<TMemoryObject> memoryObject, string location) {
            if (!memoryObject.IsSerialized)
                memoryObject.Serialize();

            using (StreamWriter sw = new StreamWriter(location)) {
                await sw.WriteAsync(memoryObject.SerializedObj);
            }
        }
    }
}