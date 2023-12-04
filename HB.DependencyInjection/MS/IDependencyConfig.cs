using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.DependencyInjection.MS
{
    public interface IDependencyConfig
    {
        void Configure(DIBuilder builder);
    }
}
