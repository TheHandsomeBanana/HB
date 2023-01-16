using HB.Utilities.Services.Caching.Exceptions;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using HB.Common.Json;
using HB.Common;
using HB.Services.Caching.Helper;

namespace HB.Utilities.Services.Caching {
    public class CachingService : ICachingService {
        private readonly System.Timers.Timer timer = new System.Timers.Timer();

        public const double TIMER_INTERVAL = 1000.0;
        public static readonly string CachePath = GlobalEnvironment.CachingService + "\\";

        private Dictionary<string, Cache> cacheTable = new Dictionary<string, Cache>();
        public IReadOnlyDictionary<string, Cache> CacheTable { get => cacheTable; }

        private List<CacheMetaInfo> cacheMetaInfos = new List<CacheMetaInfo>();
        public IReadOnlyList<CacheMetaInfo> CacheMetaInfos { get => cacheMetaInfos; }

        public CachingService() {
            LoadMetaInfo();
            timer.Interval = TIMER_INTERVAL;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        ~CachingService() {
            timer.Stop();
            timer.Dispose();

            foreach (KeyValuePair<string, Cache> kvp in CacheTable)
                Outsource(kvp.Key, kvp.Value);

            foreach (CacheMetaInfo cacheMetaInfo in cacheMetaInfos)
                Outsource(cacheMetaInfo);
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
            if (cacheMetaInfos.All(e => e.Key != key))
                throw new KeyNotFoundException($"{key} is not registered and cannot be reloaded.");

            if (!File.Exists(CachePath + key))
                throw new CacheException($"File {CachePath + key} does not exist, data cannot be reloaded.");

            CacheMetaInfo metaInfo = cacheMetaInfos.First(e => e.Key == key);
            using (FileStream fs = new FileStream(CachePath + key, FileMode.Open, FileAccess.Read)) {
                byte[] buffer = new byte[fs.Length];
                object cobj;

                cobj = SerializationHelper.Deserialize(fs, metaInfo.ObjectType, metaInfo.CacheType);

                cacheTable.Add(key, new Cache(cobj, metaInfo.CacheType, metaInfo.Lifetime));
            }
        }

        public void Add(string key, Cache value) {
            ArgumentNullException.ThrowIfNull(nameof(key));
            if (cacheMetaInfos.Any(e => e.Key == key))
                throw new CacheException($"{key} is already registered.");

            cacheMetaInfos.Add(new CacheMetaInfo(key, value.Value.GetType(), value.CacheType, value.Seconds));
            cacheTable.Add(key, value);
        }

        public void AddOrUpdate(string key, Cache value) {
            ArgumentNullException.ThrowIfNull(nameof(key));
            ArgumentNullException.ThrowIfNull(nameof(value));

            if (CacheMetaInfos.Any(e => e.Key == key))
                Reload(key);

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
            using (FileStream fs = new FileStream(CachePath + key, FileMode.OpenOrCreate, FileAccess.Write))
                cache.Serialize(fs);
        }

        private void Outsource(CacheMetaInfo cacheMetaInfo) {
            using (FileStream fs = new FileStream(CachePath + cacheMetaInfo.Key + nameof(CacheMetaInfo), FileMode.OpenOrCreate, FileAccess.Write))
                cacheMetaInfo.Serialize(fs);
        }

        private void LoadMetaInfo() {
            IEnumerable<string> files = Directory.GetFiles(CachePath.TrimEnd('\\')).Where(e => e.EndsWith(nameof(CacheMetaInfo)));

            foreach(string file in files) {
                using(FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read)) {
                    cacheMetaInfos.Add((CacheMetaInfo)SerializationHelper.Deserialize(fs, typeof(CacheMetaInfo), CacheType.Json));
                }
            }
        }
    }
}
