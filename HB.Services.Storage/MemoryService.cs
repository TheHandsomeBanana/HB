using HB.Common.Serialization;
using HB.Common.Serialization.Streams;
using HB.Services.Storage.Exceptions;
using HB.Services.Security.Cryptography.Keys;

namespace HB.Services.Storage {
    public class MemoryService : IMemoryService, IExtendedMemoryService {
        public MemoryObject ReadMemory(string location, SerializerMode serializerMode) {
            string content;
            using (StreamReader sr = new StreamReader(location))
                content = sr.ReadToEnd();

            return new MemoryObject(content, serializerMode);
        }
        public MemoryObject<TObject> ReadMemory<TObject>(string location, SerializerMode serializerMode) {
            string content;
            using (StreamReader sr = new StreamReader(location))
                content = sr.ReadToEnd();

            return new MemoryObject<TObject>(content, serializerMode);
        }
        public async Task<MemoryObject> ReadMemoryAsync(string location, SerializerMode serializerMode) {
            string content;
            using (StreamReader sr = new StreamReader(location))
                content = await sr.ReadToEndAsync();

            return new MemoryObject(content, serializerMode);
        }
        public async Task<MemoryObject<TObject>> ReadMemoryAsync<TObject>(string location, SerializerMode serializerMode) {
            string content;
            using (StreamReader sr = new StreamReader(location))
                content = await sr.ReadToEndAsync();

            return new MemoryObject<TObject>(content, serializerMode);
        }
        public void WriteMemory(MemoryObject memoryObject, string location) {
            if (!memoryObject.IsSerialized)
                memoryObject.Serialize();

            using (StreamWriter sw = new StreamWriter(location)) {
                sw.Write(memoryObject.SerializedObj);
            }
        }
        public void WriteMemory<TObject>(MemoryObject<TObject> memoryObject, string location) {
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
        public async Task WriteMemoryAsync<TObject>(MemoryObject<TObject> memoryObject, string location) {
            if (!memoryObject.IsSerialized)
                memoryObject.Serialize();

            using (StreamWriter sw = new StreamWriter(location)) {
                await sw.WriteAsync(memoryObject.SerializedObj);
            }
        }


        TObject IExtendedMemoryService.ReadMemory<TObject>(string location, SerializerMode serializerMode) {
            MemoryObject<TObject> mo = ReadMemory<TObject>(location, serializerMode);
            return mo.Deserialize() ?? throw new MemoryServiceException("Failed reading memory.");
        }
        public TObject ReadMemory<TObject>(string location, SerializerMode serializerMode, IKey key) {
            MemoryObject<TObject> mo = ReadMemory<TObject>(location, serializerMode);
            return mo.Deserialize(key) ?? throw new MemoryServiceException("Failed reading memory.");
        }
        async Task<TObject> IExtendedMemoryService.ReadMemoryAsync<TObject>(string location, SerializerMode serializerMode) {
            MemoryObject<TObject> mo = await ReadMemoryAsync<TObject>(location, serializerMode);
            return mo.Deserialize() ?? throw new MemoryServiceException("Failed reading memory.");
        }
        public async Task<TObject> ReadMemoryAsync<TObject>(string location, SerializerMode serializerMode, IKey key) {
            MemoryObject<TObject> mo = await ReadMemoryAsync<TObject>(location, serializerMode);
            return mo.Deserialize(key) ?? throw new MemoryServiceException("Failed reading memory.");
        }
        public void WriteMemory<TObject>(string location, TObject obj, SerializerMode serializerMode) {
            MemoryObject<TObject> mo = new MemoryObject<TObject>(obj, serializerMode);
            mo.Serialize();
            WriteMemory(mo, location);
        }
        public void WriteMemory<TObject>(string location, TObject obj, SerializerMode serializerMode, IKey key) {
            MemoryObject<TObject> mo = new MemoryObject<TObject>(obj, serializerMode);
            mo.Serialize(key);
            WriteMemory(mo, location);
        }
        public async Task WriteMemoryAsync<TObject>(string location, TObject obj, SerializerMode serializerMode) {
            MemoryObject<TObject> mo = new MemoryObject<TObject>(obj, serializerMode);
            mo.Serialize();
            await WriteMemoryAsync(mo, location);
        }
        public async Task WriteMemoryAsync<TObject>(string location, TObject obj, SerializerMode serializerMode, IKey key) {
            MemoryObject<TObject> mo = new MemoryObject<TObject>(obj, serializerMode);
            mo.Serialize(key);
            await WriteMemoryAsync(mo, location);
        }
    }
}