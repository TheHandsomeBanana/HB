using HB.NETF.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Storage {
    public interface ISerializerService {
        SerializerObject Read(string location, SerializerMode serializerMode);
        Task<SerializerObject> ReadAsync(string location, SerializerMode serializerMode);
        void Write(SerializerObject memoryObject, string location);
        Task WriteAsync(SerializerObject memoryObject, string location);

        SerializerObject<TObject> Read<TObject>(string location, SerializerMode serializerMode);
        Task<SerializerObject<TObject>> ReadAsync<TObject>(string location, SerializerMode serializerMode);
        void Write<TObject>(SerializerObject<TObject> memoryObject, string location);
        Task WriteAsync<TObject>(SerializerObject<TObject> memoryObject, string location);
    }
}
