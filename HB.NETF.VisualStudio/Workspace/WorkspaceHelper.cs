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

        // Bad according to https://stackoverflow.com/questions/31194968/how-to-register-my-service-as-a-global-service-or-how-can-i-use-mef-in-my-scenar
        // Use SComponentModel & SVsUIShell and 
        public static IComponentModel GetComponentModel() => (IComponentModel)Package.GetGlobalService(typeof(IComponentModel));
        
        public static IComponentModel GetServiceComponentModel() => (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));

        public static IVsUIShell GetUIShell() {
            ThreadHelper.ThrowIfNotOnUIThread();
            return (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
        }

        public static VisualStudioWorkspace GetVisualStudioWorkspace() => GetServiceComponentModel().GetService<VisualStudioWorkspace>();

        public static Solution GetSolution() => GetDTE().Solution;
        public static Microsoft.CodeAnalysis.Solution GetCurrentCASolution() => GetVisualStudioWorkspace().CurrentSolution;
        
    }
}
