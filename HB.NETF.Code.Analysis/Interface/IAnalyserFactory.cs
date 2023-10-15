using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Interface {
    public interface IAnalyserFactory {
        IReadOnlyDictionary<Type, ICodeAnalyser> AnalyserContainer { get; }
        ICodeAnalyser CreateAnalyser(Type analyserType, Solution solution, Project project, SemanticModel semanticModel);
        ICodeAnalyser CreateAnalyser<TAnalyser>(Solution solution, Project project, SemanticModel semanticModel);
        ICodeAnalyser<TResult> CreateAnalyser<TResult>(Type analyserType, Solution solution, Project project, SemanticModel semanticModel);
        ICodeAnalyser<TResult> CreateAnalyser<TAnalyser, TResult>(Solution solution, Project project, SemanticModel semanticModel);

        ITypeAnalyser CreateTypeAnalyser(Solution solution, Project project, SemanticModel semanticModel, Type[] typeFilter);
        IIdentifierAnalyser CreateIdentifierAnalyser(Solution solution, Project project, SemanticModel semanticModel);
        IVariableAnalyser CreateVariableAnalyser(Solution solution, Project project, SemanticModel semanticModel);

        SemanticModelCache CreateSemanticModelCache(IImmutableSet<Document> documents);

        ICodeAnalyser GetAnalyser(Type analyserType);
        ICodeAnalyser GetAnalyzer<TAnalyser>();
        ICodeAnalyser<TResult> GetAnalyser<TResult>(Type analyserType);
        ICodeAnalyser<TResult> GetAnalyser<TAnalyser, TResult>();

        void Reset();
    }
}
