using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Utilities.Services.Caching {
    public struct CacheMetaInfo {
        public string Key { get; set; }
        public Type ObjectType { get; set; }
        public CacheType CacheType { get; set; }
        public double? Lifetime { get; set; }

        public CacheMetaInfo(string key, Type objectType, CacheType cacheType, double? lifetime) {
            ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
            ArgumentNullException.ThrowIfNull(objectType, nameof(objectType));

            Key = key;
            ObjectType = objectType;
            CacheType = cacheType;
            Lifetime = lifetime;
        }
    }
}
