using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.DataHandling {
    public interface ISecurityStreamHandler<TItem> : IDisposable {
        TItem? Read();
        void Write(TItem item);
        Task<TItem?> ReadAsync();
        Task WriteAsync(TItem item);
    }
}
