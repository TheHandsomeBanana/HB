using HB.NETF.Common.Serialization;
using HB.NETF.Common.Serialization.Streams;
using HB.NETF.Services.Storage.Exceptions;
using HB.NETF.Services.Security.Cryptography.Keys;
using System.IO;
using System.Threading.Tasks;

namespace HB.NETF.Services.Storage {
    public class MemoryService : IMemoryService, ISimplifiedMemoryService {
        public const string MemoryExtension = ".hbmf";
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


        TObject ISimplifiedMemoryService.ReadMemory<TObject>(string location, SerializerMode serializerMode) {
            MemoryObject<TObject> mo = ReadMemory<TObject>(location, serializerMode);
            TObject result = mo.Deserialize();
            if (result == null)
                throw new MemoryServiceException("Failed reading memory.");

            return result;
        }
        TObject ISimplifiedMemoryService.ReadMemory<TObject>(string location, SerializerMode serializerMode, IKey key) {
            MemoryObject<TObject> mo = ReadMemory<TObject>(location, serializerMode);
            TObject result = mo.Deserialize(key);
            if (result == null)
                throw new MemoryServiceException("Failed reading memory.");

            return result;
        }
        async Task<TObject> ISimplifiedMemoryService.ReadMemoryAsync<TObject>(string location, SerializerMode serializerMode) {
            MemoryObject<TObject> mo = await ReadMemoryAsync<TObject>(location, serializerMode);
            TObject result = mo.Deserialize();
            if (result == null)
                throw new MemoryServiceException("Failed reading memory.");

            return result;
        }
        async Task<TObject> ISimplifiedMemoryService.ReadMemoryAsync<TObject>(string location, SerializerMode serializerMode, IKey key) {
            MemoryObject<TObject> mo = await ReadMemoryAsync<TObject>(location, serializerMode);
            TObject result = mo.Deserialize(key);
            if (result == null)
                throw new MemoryServiceException("Failed reading memory.");

            return result;
        }
        void ISimplifiedMemoryService.WriteMemory<TObject>(string location, TObject obj, SerializerMode serializerMode) {
            MemoryObject<TObject> mo = new MemoryObject<TObject>(obj, serializerMode);
            mo.Serialize();
            WriteMemory(mo, location);
        }
        void ISimplifiedMemoryService.WriteMemory<TObject>(string location, TObject obj, SerializerMode serializerMode, IKey key) {
            MemoryObject<TObject> mo = new MemoryObject<TObject>(obj, serializerMode);
            mo.Serialize(key);
            WriteMemory(mo, location);
        }
        async Task ISimplifiedMemoryService.WriteMemoryAsync<TObject>(string location, TObject obj, SerializerMode serializerMode) {
            MemoryObject<TObject> mo = new MemoryObject<TObject>(obj, serializerMode);
            mo.Serialize();
            await WriteMemoryAsync(mo, location);
        }
        async Task ISimplifiedMemoryService.WriteMemoryAsync<TObject>(string location, TObject obj, SerializerMode serializerMode, IKey key) {
            MemoryObject<TObject> mo = new MemoryObject<TObject>(obj, serializerMode);
            mo.Serialize(key);
            await WriteMemoryAsync(mo, location);
        }
    }
}