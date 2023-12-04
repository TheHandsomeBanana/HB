using EnvDTE;
using HB.NETF.Common.Exceptions;
using Microsoft.IO;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HB.NETF.VisualStudio.Workspace {
    public static class ProjectHelper {
        public static void AddExistingFile(Project project, string fileName) {
            ThreadHelper.ThrowIfNotOnUIThread();
            fileName = Path.GetFullPath(fileName);

            if (!Path.IsPathRooted(fileName))
                throw new InternalException($"{nameof(fileName)} is not rooted. Provide absolute filepath");

            if (!ContainsFile(project, fileName))
                project.ProjectItems.AddFromFile(fileName);
        }

        public static bool ContainsFile(Project project, string fileName) => GetProjectFiles(project).Contains(fileName);

        public static IEnumerable<string> GetProjectFiles(Project project) {
            ThreadHelper.ThrowIfNotOnUIThread();

            return GetProjectFiles(project.ProjectItems);
        }

        public static IEnumerable<string> GetProjectFiles(ProjectItems projectitems) {
            foreach (ProjectItem pi in projectitems) {
                for (short i = 1; i <= projectitems.Count; i++)
                    yield return pi.FileNames[i];

                foreach (string file in GetProjectFiles(pi.ProjectItems))
                    yield return file;
            }
        }

        public static IEnumerable<string> GetFilesNotInProject(Project project) => GetFilesNotInProjectByPattern(project, "*");

        public static IEnumerable<string> GetProjectFilesByPattern(Project project, string pattern) {
            ThreadHelper.ThrowIfNotOnUIThread();
            foreach (string file in Directory.GetFiles(Path.GetDirectoryName(project.FullName), pattern, SearchOption.AllDirectories)) {
                if (GetProjectFiles(project).Contains(file))
                    yield return file;
            }
        }

        public static IEnumerable<string> GetFilesNotInProjectByPattern(Project project, string pattern) {
            ThreadHelper.ThrowIfNotOnUIThread();
            foreach (string file in Directory.GetFiles(Path.GetDirectoryName(project.FullName), pattern, SearchOption.AllDirectories)) {
                if (!GetProjectFiles(project).Contains(file))
                    yield return file;
            }
        }
    }
}
