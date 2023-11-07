﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.WPF.ViewModels {
    public interface ICloseableWindow {
        Action Close { get; set; }
        bool CanClose();
    }
}