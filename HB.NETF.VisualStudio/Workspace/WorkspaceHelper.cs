using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.VisualStudio.Workspace {
    public static class WorkspaceHelper {
        public static DTE GetDTE() {
            ThreadHelper.ThrowIfNotOnUIThread();

            return (DTE)Package.GetGlobalService(typeof(DTE));
        }

        public static IComponentModel GetComponentModel() => (IComponentModel)Package.GetGlobalService(typeof(IComponentModel));
        public static SComponentModel GetServiceComponentModel() => (SComponentModel)Package.GetGlobalService(typeof(SComponentModel));

        public static IVsUIShell GetUIShell() {
            ThreadHelper.ThrowIfNotOnUIThread();
            return (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
        }

        public static VisualStudioWorkspace GetVisualStudioWorkspace() => GetComponentModel().GetService<VisualStudioWorkspace>();

        public static Solution GetSolution() => GetDTE().Solution;
        public static Microsoft.CodeAnalysis.Solution GetCurrentCASolution() => GetVisualStudioWorkspace().CurrentSolution;
        
    }
}
