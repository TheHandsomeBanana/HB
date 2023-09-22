using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Serialization.Json {
    public static class JsonExtensions {
        /// <summary>
        /// Serialized the given <paramref name="value"/> and writes the JSON structure to a file with the given <see cref="FileStream"/> <paramref name="fs"/>.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="fs"></param>
        /// <param name="value"></param>
        public static void Serialize(this JsonSerializer serializer, FileStream fs, object value) {
            using (StreamWriter sw = new StreamWriter(fs))
                serializer.Serialize(sw, value);
        }

        /// <summary>
        /// Deserializes the JSON structure contained by the given <see cref="FileStream"/> <paramref name="fs"/> into the specified <paramref name="objectType"/>.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="fs"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static object Deserialize(this JsonSerializer serializer, FileStream fs, Type objectType) {
            using (StreamReader sr = new StreamReader(fs))
                return serializer.Deserialize(sr, objectType);
        }
    }
}
