using System.Collections.Generic;

namespace HB.NETF.Services.Data.Holder {
    public class DataHolder : IDataHolder {
        private readonly IDictionary<string, object> data = new Dictionary<string, object>();
        public T Get<T>() where T : new() => Get<T>(typeof(T).FullName);

        public T Get<T>(string key) where T : new() {
            if (!data.ContainsKey(key))
                return default;

            try {
                return (T)data[key];
            }
            catch {
                return default;
            }
        }

        public void Hold<T>(T data) where T : new() => Hold(typeof(T).FullName, data);
        public void Hold<T>(string key, T data) where T : new() => this.data[key] = data;
        public bool Has<T>() => Has(typeof(T).FullName);
        public bool Has(string key) => this.data.ContainsKey(key);
    }

    public class DataHolder<T> : IDataHolder<T> where T : new() {
        private readonly IDictionary<string, T> data = new Dictionary<string, T>();

        public T Get(string key) {
            if (!data.ContainsKey(key))
                return new T();

            return data[key];
        }

        public void Hold(string key, T data) => this.data[key] = data;

        public bool Has(string key) => data.ContainsKey(key);
    }
}
