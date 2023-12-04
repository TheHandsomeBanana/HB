namespace HB.NETF.Services.Data.Holder {
    public interface IDataHolder {
        void Hold<T>(T data) where T : new();
        T Get<T>() where T : new();
        void Hold<T>(string key, T data) where T : new();
        T Get<T>(string key) where T : new();
        bool Has<T>();
        bool Has(string key);
    }

    public interface IDataHolder<T> where T : new() {
        void Hold(string key, T data);
        T Get(string key);
        bool Has(string key);
    }
}
