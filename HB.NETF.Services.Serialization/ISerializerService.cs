using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Serialization {
    public interface ISerializerService {
        void Serialize<T>(T data, SerializerMode mode);
        T Deserialize<T>(string data, SerializerMode mode);
    }
}
