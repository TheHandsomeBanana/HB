using HB.NETF.Common.Serialization;
using HB.NETF.Common.Serialization.Streams;
using HB.NETF.Services.Storage.Exceptions;
using HB.NETF.Services.Security.Cryptography.Keys;
using System.IO;
using System.Threading.Tasks;

namespace HB.NETF.Services.Storage {
    public class SerializerService : ISerializerService, ISimplifiedSerializerService {
        public const string MemoryExtension = ".hbsf";
        public SerializerObject Read(string location, SerializerMode serializerMode) {
            string content;
            using (StreamReader sr = new StreamReader(location))
                content = sr.ReadToEnd();

            return new SerializerObject(content, serializerMode);
        }
        public SerializerObject<TObject> Read<TObject>(string location, SerializerMode serializerMode) {
            string content;
            using (StreamReader sr = new StreamReader(location))
                content = sr.ReadToEnd();

            return new SerializerObject<TObject>(content, serializerMode);
        }
        public async Task<SerializerObject> ReadAsync(string location, SerializerMode serializerMode) {
            string content;
            using (StreamReader sr = new StreamReader(location))
                content = await sr.ReadToEndAsync();

            return new SerializerObject(content, serializerMode);
        }
        public async Task<SerializerObject<TObject>> ReadAsync<TObject>(string location, SerializerMode serializerMode) {
            string content;
            using (StreamReader sr = new StreamReader(location))
                content = await sr.ReadToEndAsync();

            return new SerializerObject<TObject>(content, serializerMode);
        }
        public void Write(SerializerObject serializerObject, string location) {
            if (!serializerObject.IsSerialized)
                serializerObject.Serialize();

            using (StreamWriter sw = new StreamWriter(location)) {
                sw.Write(serializerObject.SerializedObj);
            }
        }
        public void Write<TObject>(SerializerObject<TObject> serializerObject, string location) {
            if (!serializerObject.IsSerialized)
                serializerObject.Serialize();

            using (StreamWriter sw = new StreamWriter(location)) {
                sw.Write(serializerObject.SerializedObj);
            }
        }
        public async Task WriteAsync(SerializerObject serializerObject, string location) {
            if (!serializerObject.IsSerialized)
                serializerObject.Serialize();

            using (StreamWriter sw = new StreamWriter(location)) {
                await sw.WriteAsync(serializerObject.SerializedObj);
            }
        }
        public async Task WriteAsync<TObject>(SerializerObject<TObject> serializerObject, string location) {
            if (!serializerObject.IsSerialized)
                serializerObject.Serialize();

            using (StreamWriter sw = new StreamWriter(location)) {
                await sw.WriteAsync(serializerObject.SerializedObj);
            }
        }


        TObject ISimplifiedSerializerService.Read<TObject>(string location, SerializerMode serializerMode) {
            SerializerObject<TObject> so = Read<TObject>(location, serializerMode);
            TObject result = so.Deserialize();
            if (result == null)
                throw new MemoryServiceException("Failed reading memory.");

            return result;
        }
        TObject ISimplifiedSerializerService.Read<TObject>(string location, SerializerMode serializerMode, IKey key) {
            SerializerObject<TObject> so = Read<TObject>(location, serializerMode);
            TObject result = so.Deserialize(key);
            if (result == null)
                throw new MemoryServiceException("Failed reading memory.");

            return result;
        }
        async Task<TObject> ISimplifiedSerializerService.ReadAsync<TObject>(string location, SerializerMode serializerMode) {
            SerializerObject<TObject> so = await ReadAsync<TObject>(location, serializerMode);
            TObject result = so.Deserialize();
            if (result == null)
                throw new MemoryServiceException("Failed reading memory.");

            return result;
        }
        async Task<TObject> ISimplifiedSerializerService.ReadAsync<TObject>(string location, SerializerMode serializerMode, IKey key) {
            SerializerObject<TObject> so = await ReadAsync<TObject>(location, serializerMode);
            TObject result = so.Deserialize(key);
            if (result == null)
                throw new MemoryServiceException("Failed reading memory.");

            return result;
        }
        void ISimplifiedSerializerService.Write<TObject>(string location, TObject obj, SerializerMode serializerMode) {
            SerializerObject<TObject> so = new SerializerObject<TObject>(obj, serializerMode);
            so.Serialize();
            Write(so, location);
        }
        void ISimplifiedSerializerService.Write<TObject>(string location, TObject obj, SerializerMode serializerMode, IKey key) {
            SerializerObject<TObject> so = new SerializerObject<TObject>(obj, serializerMode);
            so.Serialize(key);
            Write(so, location);
        }
        async Task ISimplifiedSerializerService.WriteAsync<TObject>(string location, TObject obj, SerializerMode serializerMode) {
            SerializerObject<TObject> so = new SerializerObject<TObject>(obj, serializerMode);
            so.Serialize();
            await WriteAsync(so, location);
        }
        async Task ISimplifiedSerializerService.WriteAsync<TObject>(string location, TObject obj, SerializerMode serializerMode, IKey key) {
            SerializerObject<TObject> so = new SerializerObject<TObject>(obj, serializerMode);
            so.Serialize(key);
            await WriteAsync(so, location);
        }
    }
}