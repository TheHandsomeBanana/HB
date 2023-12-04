using System;

namespace HB.NETF.WPF.ViewModels {
    public interface ICloseableWindow {
        Action Close { get; set; }
        bool CanClose();
    }
}
