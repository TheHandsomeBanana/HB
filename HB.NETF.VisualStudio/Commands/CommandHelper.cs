using HB.NETF.VisualStudio.Workspace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.VisualStudio.Commands {
    public static class CommandHelper {
        public static void RunVSCommand(Guid guid, uint id) {
            object pvaln = null;
            WorkspaceHelper.GetUIShell().PostExecCommand(guid, id, 0, ref pvaln);
        }
    }
}
