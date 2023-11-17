using HB.NETF.Code.Analysis.Exceptions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Tests.Engine {
    public class CodeAnalysisTest<T> {
        private readonly T presetValue;

        public Document Document { get; }
        public string SourceText { get; }
        public Dictionary<string, T> ExpectedResults { get; }

        internal CodeAnalysisTest(Document document) {
            ExpectedResults = new Dictionary<string, T>();
            Document = document;
            SourceText = File.ReadAllText(document.FilePath);
        }

        internal CodeAnalysisTest(Document document, T presetValue) : this(document) {
            this.presetValue = presetValue;
        }

        // Preset value is overriden
        public CodeAnalysisTest<T> Add(string key, T value) {
            if (ExpectedResults.ContainsKey(key))
                throw new CodeAnalyserTestException($"Expected results already contains {key}");

            ExpectedResults.Add(key, value);
            return this;
        }

        public CodeAnalysisTest<T> Add(string key) {
            if (presetValue == null)
                throw new CodeAnalyserTestException("Cannot use preset value. Preset value is not set set.");

            if (ExpectedResults.ContainsKey(key))
                throw new CodeAnalyserTestException($"Expected results already contains {key}");

            ExpectedResults.Add(key, presetValue);
            return this;
        }
    }
}
