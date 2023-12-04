using HB.NETF.Code.Analysis.Exceptions;
using HB.NETF.Code.Analysis.Factory;
using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Common.Tests;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HB.NETF.Code.Analysis.Tests {
    [TestClass]
    public class AnalyserTestBase : TestBase {
        protected IAnalyserFactory Factory { get; }

        public AnalyserTestBase() {
            Factory = new AnalyserFactory();
        }

        protected Project Project { get; private set; }
        protected Solution Solution { get; private set; }
        protected SyntaxTree SyntaxTree { get; set; }
        protected SemanticModel SemanticModel { get; set; }
        protected SyntaxNode SyntaxNode { get; set; }

        public void Initialize(string solutionPath, string projectName) {
            Solution = GetSolution(solutionPath);
            Project = Solution.Projects.FirstOrDefault(p => p.Name == projectName)
                ?? throw new CodeAnalyserTestException($"Project {projectName} not found in Solution");
        }

        protected Solution GetSolution(string solutionPath) {
            if (!MSBuildLocator.IsRegistered)
                MSBuildLocator.RegisterDefaults();
            MSBuildWorkspace w;
            try {
                w = MSBuildWorkspace.Create();
                w.WorkspaceFailed += WorkspaceFailed;
                return w.OpenSolutionAsync(solutionPath).Result;
            }
            catch (ReflectionTypeLoadException ex) {

                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions) {
                    if (exSub == null)
                        continue;

                    sb.AppendLine(exSub.Message);

                    if (exSub is FileNotFoundException exFileNotFound) {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog)) {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }

                throw new CodeAnalyserTestException(sb.ToString(), ex);
            }
        }

        protected virtual void WorkspaceFailed(object sender, WorkspaceDiagnosticEventArgs e) {
            Console.WriteLine(e.ToString());
        }
    }
}
