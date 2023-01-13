using HB.Utilities.Services.Caching.Exceptions;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using HB.Common.Json;
using HB.Common;

namespace HB.Utilities.Services.Caching {
    public class CachingService : ICachingService {
        private readonly System.Timers.Timer timer = new System.Timers.Timer();
        private readonly CachingServiceSettings cachingServiceSettings;

        public const double TIMER_INTERVAL = 1000.0;
        public static readonly string CachePath = GlobalEnvironment.CachingService + "\\";

        private Dictionary<string, Cache> cacheTable = new Dictionary<string, Cache>();
        public IReadOnlyDictionary<string, Cache> CacheTable { get => cacheTable; }

        private List<CacheMetaInfo> cacheMetaInfos = new List<CacheMetaInfo>();
        public IReadOnlyList<CacheMetaInfo> CacheMetaInfos { get => cacheMetaInfos; }

        public CachingService(CachingServiceSettings cachingServiceSettings) {
            this.cachingServiceSettings = cachingServiceSettings;
            timer.Interval = TIMER_INTERVAL;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        ~CachingService() {
            timer.Stop();

            foreach (KeyValuePair<string, Cache> kvp in CacheTable)
                Outsource(kvp.Key, kvp.Value);
        }

        public Cache Get(string key) {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return cacheTable.ContainsKey(key) ? cacheTable[key] : throw new CacheException($"Cache {key} not registered.");
        }

        public Cache? GetOrDefault(string key) {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return cacheTable.ContainsKey(key) ? cacheTable[key] : default;
        }

        public void Reload(string key) {
            if(cacheMetaInfos.All(e => e.Key != key))
                throw new KeyNotFoundException($"{key} is not registered and cannot be reloaded.");

            if (!File.Exists(CachePath + key))
                throw new CacheException($"File {CachePath + key} does not exist, data cannot be reloaded.");

            CacheMetaInfo metaInfo = cacheMetaInfos.First(e => e.Key == key);
            using (FileStream fs = new FileStream(CachePath + key, FileMode.Open, FileAccess.Read)) {
                byte[] buffer = new byte[fs.Length];
                object cobj;

                switch (metaInfo.CacheType) {
                    case CacheType.Binary:
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                        cobj = new BinaryFormatter().Deserialize(fs);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                        break;
                    case CacheType.Xml:
                        cobj = new XmlSerializer(metaInfo.ObjectType).Deserialize(fs) ?? throw new NullReferenceException("Xml deserialization returned null.");
                        break;
                    case CacheType.Json:
                        cobj = new JsonSerializer().Deserialize(fs, metaInfo.ObjectType) ?? throw new NullReferenceException("Json deserialization returned null."); ;
                        break;
                    default:
                        throw new NotSupportedException("Cache type is not supported.");
                }

                cacheTable.Add(key, new Cache(cobj, metaInfo.CacheType, metaInfo.Lifetime));
            }
        }

        public void Add(string key, Cache value) {
            ArgumentNullException.ThrowIfNull(nameof(key));
            if (cacheMetaInfos.Any(e => e.Key == key))
                throw new CacheException($"{key} is already registered.");

            cacheMetaInfos.Add(new CacheMetaInfo(key, value.Value.GetType(), value.CacheType, value.Seconds));
        }

        public void AddOrUpdate(string key, Cache value) {
            ArgumentNullException.ThrowIfNull(nameof(key));
            ArgumentNullException.ThrowIfNull(nameof(value));

            if (CacheTable.ContainsKey(key)) {
                cacheTable[key] = value;
                int metaIndex = cacheMetaInfos.IndexOf(cacheMetaInfos.First(e => e.Key == key));
                cacheMetaInfos[metaIndex] = new CacheMetaInfo(key, value.Value.GetType(), value.CacheType, value.Seconds);
            }
            else {
                cacheTable.Add(key, value);
                cacheMetaInfos.Add(new CacheMetaInfo(key, value.Value.GetType(), value.CacheType, value.Seconds));
            }
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e) {
            IEnumerable<string> removableKeys = CacheTable.Where(e => {
                e.Value.Seconds--;
                return e.Value.Seconds <= 0.0;
            }).Select(e => e.Key);

            foreach (string key in removableKeys) {
                Outsource(key, CacheTable[key]);
                cacheTable.Remove(key);
            }
        }

        private void Outsource(string key, Cache cache) {
            using (FileStream fs = new FileStream(CachePath + key, FileMode.OpenOrCreate, FileAccess.Write)) {
                switch (cache.CacheType) {
                    case CacheType.Binary:
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                        new BinaryFormatter().Serialize(fs, cache.Value);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                        break;
                    case CacheType.Xml:
                        new XmlSerializer(cache.Value.GetType()).Serialize(fs, cache.Value);
                        break;
                    case CacheType.Json:
                        new JsonSerializer().Serialize(fs, cache.Value);
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
