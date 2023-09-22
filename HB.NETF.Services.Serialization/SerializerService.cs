using System.IO;
using System.Threading.Tasks;

namespace HB.NETF.Services.Serialization {
    public class SerializerService : ISerializerService {
        public T Deserialize<T>(string data, SerializerMode mode) {
            throw new System.NotImplementedException();
        }

        public void Serialize<T>(T data, SerializerMode mode) {
            throw new System.NotImplementedException();
        }
    }
}