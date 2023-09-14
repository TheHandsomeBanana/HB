using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.VisualStudio.Workspace {
    public static class SolutionHelper {
        public static IEnumerable<Project> GetProjects(Solution solution) {
            ThreadHelper.ThrowIfNotOnUIThread();
            foreach (Project project in solution.Projects)
                yield return project;
        }

        public static Project GetCurrentProject() {
            DTE dte = WorkspaceHelper.GetDTE();

            if ((dte.SelectedItems.Item(1).Project == null && dte.SelectedItems.Item(1).ProjectItem?.ContainingProject == null) || dte.SelectedItems.Count == 0 )
                return dte.ActiveDocument?.ProjectItem?.ContainingProject;
            else
                return (dte.SelectedItems.Item(1).Project == null) ? dte.SelectedItems.Item(1).ProjectItem.ContainingProject : dte.SelectedItems.Item(1).Project;
        }

        public static Microsoft.CodeAnalysis.Project ToCAProject(Project project) {
            return WorkspaceHelper.GetCurrentCASolution().Projects.FirstOrDefault(e => {
                ThreadHelper.ThrowIfNotOnUIThread();
                return e.FilePath == project.FullName;
            });
        }

        public static Microsoft.CodeAnalysis.Project GetCurrentCAProject() => ToCAProject(GetCurrentProject());
    }
}
