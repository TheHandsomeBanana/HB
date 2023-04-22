using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Common.DependencyInjection {
    public interface IDependencyConfig {
        void Configure(DIBuilder builder);
    }
}
