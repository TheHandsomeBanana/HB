using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.DataHandling {
    [Obsolete]
    public interface ISecurityStreamHandler<TItem> : IDisposable {
        [Obsolete]
        TItem Read();
        [Obsolete]
        void Write(TItem item);
        [Obsolete]
        Task<TItem> ReadAsync();
        [Obsolete]
        Task WriteAsync(TItem item);
    }
}
