using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.DependencyInjection.MS
{
    public class DIBuilder
    {
        public ServiceCollection Services { get; } = new ServiceCollection();
    }
}
