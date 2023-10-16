using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Interface {
    /// <summary>
    /// Factory for <see cref="ICodeAnalyser"/> management.
    /// </summary>
    public interface IAnalyserFactory {
        SemanticModelCache SemanticModelCache { get; }
        IReadOnlyDictionary<AnalyserFactoryKey, ICodeAnalyser> AnalyserContainer { get; }
        ICodeAnalyser GetOrCreateAnalyser(Type analyserType, Solution solution, Project project, SemanticModel semanticModel);
        TAnalyser GetOrCreateAnalyser<TAnalyser>(Solution solution, Project project, SemanticModel semanticModel) where TAnalyser : ICodeAnalyser;
        ICodeAnalyser<TResult> GetOrCreateAnalyser<TResult>(Type analyserType, Solution solution, Project project, SemanticModel semanticModel);

        ITypeAnalyser GetOrCreateTypeAnalyser(Solution solution, Project project, SemanticModel semanticModel, Type[] typeFilter);

        void SetSemanticModelCache(IImmutableSet<Document> documents);

        IEnumerable<ICodeAnalyser> GetAnalysers(Type analyserType);
        IEnumerable<TAnalyser> GetAnalysers<TAnalyser>() where TAnalyser : ICodeAnalyser;
        IEnumerable<ICodeAnalyser<TResult>> GetAnalysers<TResult>(Type analyserType);

        ICodeAnalyser GetAnalyser(AnalyserFactoryKey key);
        TAnalyser GetAnalyser<TAnalyser>(string filePath);
        ICodeAnalyser<TResult> GetAnalyser<TResult>(AnalyserFactoryKey key);

        void Reset();
    }

    /// <summary>
    /// Unique key for an Analyser given by <see cref="Type"/> and <see cref="Microsoft.CodeAnalysis.SemanticModel"/>
    /// </summary>
    public readonly struct AnalyserFactoryKey : IEquatable<AnalyserFactoryKey> {
        public Type AnalyserType { get; }
        public string FilePath { get; }

        public AnalyserFactoryKey(Type analyserType, string filePath) {
            this.AnalyserType = analyserType;
            this.FilePath = filePath;
        }

        public static bool operator ==(AnalyserFactoryKey left, AnalyserFactoryKey right) {
            return left.Equals(right);
        }

        public static bool operator !=(AnalyserFactoryKey left, AnalyserFactoryKey right) {
            return !left.Equals(right);
        }

        public override bool Equals(object obj) {
            return obj is AnalyserFactoryKey key && Equals(key);
        }

        public override int GetHashCode() {
            return this.AnalyserType.GetHashCode() + this.FilePath.GetHashCode();
        }

        public bool Equals(AnalyserFactoryKey other) {
            return this.AnalyserType == other.AnalyserType && FilePath == other.FilePath;
        }
    }
}
