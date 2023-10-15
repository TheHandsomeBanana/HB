using HB.NETF.Code.Analysis.Analyser;
using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Factory {
    public class AnalyserFactory : IAnalyserFactory {
        private Dictionary<Type, ICodeAnalyser> analyserContainer = new Dictionary<Type, ICodeAnalyser>();
        public IReadOnlyDictionary<Type, ICodeAnalyser> AnalyserContainer => analyserContainer;

        public ICodeAnalyser CreateAnalyser(Type analyserType, Solution solution, Project project, SemanticModel semanticModel) {
            if(AnalyserContainer.ContainsKey(analyserType))
                return analyserContainer[analyserType];

            ICodeAnalyser analyser = Activator.CreateInstance(analyserType, solution, project, semanticModel) as ICodeAnalyser;
            analyserContainer.Add(analyserType, analyser);
            return analyser;
        }

        public ICodeAnalyser CreateAnalyser<TAnalyser>(Solution solution, Project project, SemanticModel semanticModel) {
            return CreateAnalyser(typeof(TAnalyser), solution, project, semanticModel);
        }

        public ICodeAnalyser<TResult> CreateAnalyser<TResult>(Type analyserType, Solution solution, Project project, SemanticModel semanticModel) {
            return CreateAnalyser(analyserType, solution, project, semanticModel) as ICodeAnalyser<TResult>;
        }

        public ICodeAnalyser<TResult> CreateAnalyser<TAnalyser, TResult>(Solution solution, Project project, SemanticModel semanticModel) {
            return CreateAnalyser<TAnalyser>(solution, project, semanticModel) as ICodeAnalyser<TResult>;
        }

        public SemanticModelCache CreateSemanticModelCache(IImmutableSet<Document> documents) {
            return new SemanticModelCache(documents);
        }

        public ITypeAnalyser CreateTypeAnalyser(Solution solution, Project project, SemanticModel semanticModel, Type[] typeFilter) {
            return new TypeAnalyser(solution, project, semanticModel, typeFilter);
        }

        public IIdentifierAnalyser CreateIdentifierAnalyser(Solution solution, Project project, SemanticModel semanticModel) {
            return new IdentifierAnalyser(solution, project, semanticModel);
        }

        public IVariableAnalyser CreateVariableAnalyser(Solution solution, Project project, SemanticModel semanticModel) {
            return new VariableAnalyser(solution, project, semanticModel);
        }

        public ICodeAnalyser GetAnalyser(Type analyserType) {
            if (!analyserContainer.ContainsKey(analyserType))
                return null;

            return analyserContainer[analyserType];
        }

        public ICodeAnalyser<TResult> GetAnalyser<TResult>(Type analyserType) {
            return GetAnalyser(analyserType) as ICodeAnalyser<TResult>;
        }

        public ICodeAnalyser<TResult> GetAnalyser<TAnalyser, TResult>() {
            return GetAnalyser<TResult>(typeof(TAnalyser));
        }

        public ICodeAnalyser GetAnalyzer<TAnalyser>() {
            return GetAnalyser(typeof(TAnalyser));
        }

        public void Reset() {
            analyserContainer.Clear();
        }
    }
}
