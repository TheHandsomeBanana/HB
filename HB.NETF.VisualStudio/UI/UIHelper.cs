using HB.NETF.Common.DependencyInjection;
using HB.NETF.Services.Logging;
using HB.NETF.VisualStudio.Workspace;
using Microsoft;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.Internal.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HB.NETF.VisualStudio.UI {
    public static class UIHelper {
        public static IServiceProvider Package { get; set; }

        public static void Show(System.Windows.Window window) {
            IVsUIShell uiShell = WorkspaceHelper.GetUIShell();
            Assumes.Present(uiShell);

            uiShell.GetDialogOwnerHwnd(out IntPtr hwnd);
            WindowHelper.ShowModal(window, hwnd);
        }

        public static void ShowInfo(string message, string title) {
            VsShellUtilities.ShowMessageBox(
                Package,
                message,
                title,
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        public static void ShowWarning(string message, string title) {
            VsShellUtilities.ShowMessageBox(
                Package,
                message,
                title,
                OLEMSGICON.OLEMSGICON_WARNING,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        public static void ShowError(string message, string title) {
            VsShellUtilities.ShowMessageBox(
               Package,
               message,
               title,
               OLEMSGICON.OLEMSGICON_CRITICAL,
               OLEMSGBUTTON.OLEMSGBUTTON_OK,
               OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        public static int ShowWarningWithCancel(string message, string title) {
            return VsShellUtilities.ShowMessageBox(
                Package,
                message,
                title,
                OLEMSGICON.OLEMSGICON_WARNING,
                OLEMSGBUTTON.OLEMSGBUTTON_OKCANCEL,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        private static IVsOutputWindowPane pane;
        public static void InitOutputLog(string paneName) {
            ThreadHelper.ThrowIfNotOnUIThread();

            IVsOutputWindow logWindow = AsyncPackage.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            Guid guid = Guid.NewGuid();
            logWindow.CreatePane(ref guid, paneName, 1, 1);
            logWindow.GetPane(ref guid, out pane);
        }

        public static void OutputWindowFunc(LogStatement obj) {
            Func<Task> logTask = new Func<Task>(async () => {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                pane.OutputStringThreadSafe(obj.ToString() + "\n");
            });

            ThreadHelper.JoinableTaskFactory.Run(logTask);
        }
    }
}
