using HB.Code.Analysis.Exceptions;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Analysis.TestEngine {
    public abstract class CodeAnalysisTestEngine<TValue> {
        protected Project Project { get; }
        protected Solution Solution { get; }
        protected SyntaxTree? SyntaxTree { get; private set; }
        protected SemanticModel? SemanticModel { get; private set; }
        protected SyntaxNode? SyntaxNode { get; private set; }
        
        private List<CodeAnalysisTest<TValue>> tests { get; } = new List<CodeAnalysisTest<TValue>>();
        protected IReadOnlyList<CodeAnalysisTest<TValue>> Tests { get => tests; }
        protected List<string> Errors { get; } = new List<string>();

        public CodeAnalysisTestEngine(string solutionPath, string projectName) {
            Solution = GetSolution(solutionPath);
            Project = Solution.Projects.FirstOrDefault(p => p.Name == projectName) 
                ?? throw new CodeAnalyserTestException($"Project {projectName} not found in Solution");
        }

        public CodeAnalysisTest<TValue> Add(string fileName, TValue value) {
            Document? document = Project.Documents.FirstOrDefault(e => e.Name == fileName)
                ?? throw new CodeAnalyserTestException($"File {fileName} not found. Missing '.cs'?");

            CodeAnalysisTest<TValue> item = new CodeAnalysisTest<TValue>(document, value);
            tests.Add(item);
            return item;
        }

        public CodeAnalysisTest<TValue> Add(string fileName) {
            Document? document = Project.Documents.FirstOrDefault(e => e.Name == fileName)
                ?? throw new CodeAnalyserTestException($"File {fileName} not found. Missing '.cs'?");

            CodeAnalysisTest<TValue> item = new CodeAnalysisTest<TValue>(document);
            tests.Add(item);
            return item;
        }

        protected abstract Task RunTestAsync(string key, TValue value);


        public async Task RunEngineAsync() {
            foreach (CodeAnalysisTest<TValue> test in Tests) {
                await ResolveCompilation(test.Document);

                foreach (var result in test.ExpectedResults) {
                    await InitTestRun(result.Key, test.SourceText, test.Document.Name);
                    await RunTestAsync(result.Key, result.Value);
                }
            }

            Assert.AreEqual(0, Errors.Count, string.Join("\n", Errors));
        }

        private async Task ResolveCompilation(Document document) {
            Compilation compilation = await Project.GetCompilationAsync()
                ?? throw new CodeAnalyserTestException("Could not load compilation.");

            SyntaxTree = await document.GetSyntaxTreeAsync()
                ?? throw new CodeAnalyserTestException("Could not load syntax tree.");

            SemanticModel = compilation.GetSemanticModel(SyntaxTree)
                ?? throw new CodeAnalyserTestException("Could not load semantic model");
        }

        private async Task InitTestRun(string testString, string sourceText, string documentName) {
            int pos = sourceText.IndexOf(testString);
            if (pos == -1)
                throw new CodeAnalyserTestException("Value not found: " + documentName + ": " + testString);            

            SyntaxNode = (await SyntaxTree!.GetRootAsync()).FindNode(new TextSpan(pos, 1));
        }

        private Solution GetSolution(string solutionPath) {
            if (!MSBuildLocator.IsRegistered)
                MSBuildLocator.RegisterDefaults();
            MSBuildWorkspace w;

            try {
                w = MSBuildWorkspace.Create();
                return w.OpenSolutionAsync(solutionPath).Result;
            }
            catch (ReflectionTypeLoadException ex) {

                StringBuilder sb = new StringBuilder();
                foreach (Exception? exSub in ex.LoaderExceptions) {
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
    }
}
