using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Common.TimeTracker {
    public interface ITimeTracker {
        void Track(string context);
        void StopTrack(string context);
        void PauseTrack(string context);
        void ResumeTrack(string context);
    }
}
