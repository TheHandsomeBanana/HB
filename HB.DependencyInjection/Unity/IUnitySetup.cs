﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace HB.DependencyInjection.Unity {
    public interface IUnitySetup {
        void Build(IUnityContainer container);
    }
}
