using HB.NETF.Code.Analysis.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Tests.Engine {
    public abstract class CodeAnalysisTestEngine<T> : AnalyserTestBase {
        private readonly List<CodeAnalysisTest<T>> tests = new List<CodeAnalysisTest<T>>();
        protected IReadOnlyList<CodeAnalysisTest<T>> Tests { get => tests; }
        protected List<string> Errors { get; } = new List<string>();

        public CodeAnalysisTest<T> Add(string fileName, T value) {
            Document document = Project.Documents.FirstOrDefault(e => e.Name == fileName)
                ?? throw new CodeAnalyserTestException($"File {fileName} not found.");

            CodeAnalysisTest<T> item = new CodeAnalysisTest<T>(document, value);
            tests.Add(item);
            return item;
        }

        public CodeAnalysisTest<T> Add(string fileName) {
            Document document = Project.Documents.FirstOrDefault(e => e.Name == fileName)
                ?? throw new CodeAnalyserTestException($"File {fileName} not found.");

            CodeAnalysisTest<T> item = new CodeAnalysisTest<T>(document);
            tests.Add(item);
            return item;
        }

        public void Reset() {
            tests.Clear();
            Errors.Clear();
        }

        protected abstract Task RunTestAsync(string key, T value, string document);

        public async Task RunEngineAsync() {
            Compilation compilation = await Project.GetCompilationAsync()
                ?? throw new CodeAnalyserTestException("Could not load compilation.");

            foreach (CodeAnalysisTest<T> test in Tests) {
                SyntaxTree = await test.Document.GetSyntaxTreeAsync()
                    ?? throw new CodeAnalyserTestException("Could not load syntax tree.");

                SemanticModel = compilation.GetSemanticModel(SyntaxTree);

                foreach (var result in test.ExpectedResults) {
                    await InitTestRun(result.Key, test.SourceText, test.Document.Name);
                    await RunTestAsync(result.Key, result.Value, test.Document.Name);
                }
            }

            Assert.AreEqual(0, Errors.Count, string.Join("\n", Errors));
        }

        private async Task InitTestRun(string testString, string sourceText, string documentName) {
            int pos = sourceText.IndexOf(testString);
            if (pos == -1)
                throw new CodeAnalyserTestException("Value not found: " + documentName + ": " + testString);

            SyntaxNode = (await SyntaxTree.GetRootAsync()).FindNode(new TextSpan(pos, 1));
        }
    }
}
