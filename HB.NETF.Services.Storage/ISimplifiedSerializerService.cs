using HB.NETF.Common.Serialization;
using HB.NETF.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Storage {
    public interface ISimplifiedSerializerService {
        TObject Read<TObject>(string location, SerializerMode serializerMode);
        TObject Read<TObject>(string location, SerializerMode serializerMode, IKey key);
        Task<TObject> ReadAsync<TObject>(string location, SerializerMode serializerMode);
        Task<TObject> ReadAsync<TObject>(string location, SerializerMode serializerMode, IKey key);

        void Write<TObject>(string location, TObject obj, SerializerMode serializerMode);
        void Write<TObject>(string location, TObject obj, SerializerMode serializerMode, IKey key);
        Task WriteAsync<TObject>(string location, TObject obj, SerializerMode serializerMode);
        Task WriteAsync<TObject>(string location, TObject obj, SerializerMode serializerMode, IKey key);
    }
}