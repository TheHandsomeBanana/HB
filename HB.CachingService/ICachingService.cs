using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Utilities.Services.Caching {
    public interface ICachingService {
        Cache Get(string key);
        Cache? GetOrDefault(string key);
        void Reload(string key);
        void Add(string key , Cache value);
        void AddOrUpdate(string key, Cache value);
    }
}
