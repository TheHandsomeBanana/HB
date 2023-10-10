using HB.Services.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Caching {
    [Serializable]
    public struct CacheMetaInfo {
        public string Key { get; set; }
        public Type ObjectType { get; set; }
        public SerializerMode CacheType { get; set; }
        public double? Lifetime { get; set; }
        

        public CacheMetaInfo(string key, Type objectType, SerializerMode cacheType, double? lifetime) {
            ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
            ArgumentNullException.ThrowIfNull(objectType, nameof(objectType));

            Key = key;
            ObjectType = objectType;
            CacheType = cacheType;
            Lifetime = lifetime;
        }
    }
}
