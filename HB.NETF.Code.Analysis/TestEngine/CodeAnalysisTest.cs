using HB.NETF.Code.Analysis.Exceptions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.TestEngine {
    public class CodeAnalysisTest<TValue> {
        private readonly TValue presetValue;

        public Document Document { get; }
        public string SourceText { get; }
        public Dictionary<string, TValue> ExpectedResults { get; }

        internal CodeAnalysisTest(Document document) {
            ExpectedResults = new Dictionary<string, TValue>();
            Document = document;
            SourceText = File.ReadAllText(document.FilePath);
        }

        internal CodeAnalysisTest(Document document, TValue presetValue) : this(document) {
            this.presetValue = presetValue;
        }

        public CodeAnalysisTest<TValue> Add(string key, TValue value) {
            if (presetValue != null)
                throw new CodeAnalyserTestException("Cannot add custom value. Preset value is set.");

            if (ExpectedResults.ContainsKey(key))
                throw new CodeAnalyserTestException($"Expected results already contains {key}");

            ExpectedResults.Add(key, value);
            return this;
        }

        public CodeAnalysisTest<TValue> Add(string key) {
            if (presetValue == null)
                throw new CodeAnalyserTestException("Cannot use preset value. Preset value is not set set.");

            if (ExpectedResults.ContainsKey(key))
                throw new CodeAnalyserTestException($"Expected results already contains {key}");

            ExpectedResults.Add(key, presetValue);
            return this;
        }
    }
}
