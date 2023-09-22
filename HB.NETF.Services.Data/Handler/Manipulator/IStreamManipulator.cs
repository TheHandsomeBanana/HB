using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Data.Handler.Manipulator {
    public interface IStreamManipulator {
        void ManipulateStream(OptionBuilderFunc optionBuilder);
    }
}
