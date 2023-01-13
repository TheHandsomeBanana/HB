using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HB.Utilities.Services.Caching {
    public class Cache {
        internal double? Seconds { get; set; } = null;
        public object Value { get; set; }
        public CacheType CacheType { get; }
        public TimeSpan? Lifetime { get => Seconds.HasValue ? TimeSpan.FromSeconds(Seconds.Value) : null; }

        public Cache(object cache, CacheType cacheType) {
            ArgumentNullException.ThrowIfNull(cache, nameof(cache));

            Value = cache;
            CacheType = cacheType;
        }

        public Cache(object cache, CacheType cacheType, double? seconds) : this(cache, cacheType) {
            this.Seconds = seconds;
        }

        public Cache(object cache, CacheType cacheType, double hours, double minutes, double seconds) : this(cache, cacheType) {
            this.Seconds = hours * 3600 + minutes * 60 + seconds;
        }
    }

    public enum CacheType {
        [Obsolete("Usage of binary formatting is dangerous and should not be used.")]
        Binary,
        Xml,
        Json
    }
}
