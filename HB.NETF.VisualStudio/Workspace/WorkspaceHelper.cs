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
        private static IVsUIShell sVsUIShell;
        public static IVsUIShell SVsUIShell {
            get {
                if (sVsUIShell == null)
                    sVsUIShell = GetUIShell();

                return sVsUIShell;
            }
        }

        private static DTE dte;
        public static DTE DTE {
            get {
                if (dte == null)
                    dte = GetDTE();

                return dte;
            }
        }

        private static IComponentModel componentModel;
        public static IComponentModel ComponentModel {
            get {
                if (componentModel == null)
                    componentModel = GetComponentModel();

                return componentModel;
            }
        }

        private static VisualStudioWorkspace visualStudioWorkspace;
        public static VisualStudioWorkspace VisualStudioWorkspace {
            get {
                if (visualStudioWorkspace == null)
                    visualStudioWorkspace = GetVisualStudioWorkspace();

                return visualStudioWorkspace;
            }
        }

        public static DTE GetDTE() {
            ThreadHelper.ThrowIfNotOnUIThread();

            return (DTE)Package.GetGlobalService(typeof(DTE));
        }

        // Deprecated according to https://stackoverflow.com/questions/31194968/how-to-register-my-service-as-a-global-service-or-how-can-i-use-mef-in-my-scenar
        // Use SComponentModel & SVsUIShell 
        //public static IComponentModel GetComponentModel() => (IComponentModel)Package.GetGlobalService(typeof(IComponentModel));
        
        public static IComponentModel GetComponentModel() => (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));

        public static IVsUIShell GetUIShell() {
            ThreadHelper.ThrowIfNotOnUIThread();
            return (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
        }

        public static VisualStudioWorkspace GetVisualStudioWorkspace() => GetComponentModel().GetService<VisualStudioWorkspace>();

        public static Solution GetSolution() => GetDTE().Solution;
        public static Microsoft.CodeAnalysis.Solution GetCurrentCASolution() => GetVisualStudioWorkspace().CurrentSolution;
        
    }
}
