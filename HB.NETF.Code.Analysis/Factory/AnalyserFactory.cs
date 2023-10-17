using HB.NETF.Code.Analysis.Analyser;
using HB.NETF.Code.Analysis.Exceptions;
using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Factory {
    public class AnalyserFactory : IAnalyserFactory {
        private readonly Dictionary<AnalyserFactoryKey, ICodeAnalyser> analyserContainer = new Dictionary<AnalyserFactoryKey, ICodeAnalyser>();
        public IReadOnlyDictionary<AnalyserFactoryKey, ICodeAnalyser> AnalyserContainer => analyserContainer;
        public SemanticModelCache SemanticModelCache { get; set; }

        public ICodeAnalyser GetOrCreateAnalyser(Type analyserType, Solution solution, Project project, SemanticModel semanticModel) {
            if (analyserType.IsInterface)
                analyserType = AnalyserTypeMapping.Get(analyserType);
            
            AnalyserFactoryKey key = new AnalyserFactoryKey(analyserType, semanticModel.SyntaxTree.FilePath);

            if (AnalyserContainer.ContainsKey(key))
                return analyserContainer[key];

            ICodeAnalyser analyser = Activator.CreateInstance(analyserType, solution, project, semanticModel) as ICodeAnalyser;
            analyserContainer.Add(key, analyser);
            return analyser;
        }

        public TAnalyser GetOrCreateAnalyser<TAnalyser>(Solution solution, Project project, SemanticModel semanticModel) where TAnalyser : ICodeAnalyser {
            try {
                return (TAnalyser)GetOrCreateAnalyser(typeof(TAnalyser), solution, project, semanticModel);
            }
            catch {
                return default;
            }
        }

        public ICodeAnalyser<TResult> GetOrCreateAnalyser<TResult>(Type analyserType, Solution solution, Project project, SemanticModel semanticModel) {
            return GetOrCreateAnalyser(analyserType, solution, project, semanticModel) as ICodeAnalyser<TResult>;
        }

        public void SetSemanticModelCache(IEnumerable<Document> documents) {
            SemanticModelCache = new SemanticModelCache(documents);
        }

        public void SetSemanticModelCache(Project project) {
            SetSemanticModelCache(project.Documents);
        }

        public void SetSemanticModelCache(Solution solution) {
            SetSemanticModelCache(solution.Projects.SelectMany(e => e.Documents));
        }

        public ITypeAnalyser GetOrCreateTypeAnalyser(Solution solution, Project project, SemanticModel semanticModel, Type[] typeFilter) {
            AnalyserFactoryKey key = new AnalyserFactoryKey(typeof(TypeAnalyser), semanticModel.SyntaxTree.FilePath);

            if (AnalyserContainer.ContainsKey(key))
                return (TypeAnalyser)analyserContainer[key];

            ITypeAnalyser analyser = new TypeAnalyser(solution, project, semanticModel, typeFilter);
            analyserContainer.Add(key, analyser);
            return analyser;
        }

        public IEnumerable<ICodeAnalyser> GetAnalysers(Type analyserType) {
            return analyserContainer.Where(e => e.Key.AnalyserType == analyserType).Select(e => e.Value);
        }

        public IEnumerable<TAnalyser> GetAnalysers<TAnalyser>() where TAnalyser : ICodeAnalyser {
            return GetAnalysers(typeof(TAnalyser)).Cast<TAnalyser>();
        }

        public IEnumerable<ICodeAnalyser<TResult>> GetAnalysers<TResult>(Type analyserType) {
            return GetAnalysers(analyserType).Cast<ICodeAnalyser<TResult>>();
        }

        public IEnumerable<ICodeAnalyser<TResult>> GetAnalysers<TAnalyser, TResult>() where TAnalyser : ICodeAnalyser {
            return GetAnalysers<TResult>(typeof(TAnalyser));
        }

        public ICodeAnalyser GetAnalyser(AnalyserFactoryKey key) {
            if (!analyserContainer.ContainsKey(key))
                return null;

            return analyserContainer[key];
        }

        public TAnalyser GetAnalyser<TAnalyser>(string filePath) {
            try {
                return (TAnalyser)GetAnalyser(new AnalyserFactoryKey(typeof(TAnalyser), filePath));
            }
            catch {
                return default;
            }
        }

        public ICodeAnalyser<TResult> GetAnalyser<TResult>(AnalyserFactoryKey key) {
            return GetAnalyser(key) as ICodeAnalyser<TResult>;
        }

        public void Reset() {
            analyserContainer.Clear();
        }

        /// <summary>
        /// Helper class, reflection type loading takes way too much time
        /// </summary>
        static class AnalyserTypeMapping {
            public static Type Get(Type type) {
                switch(type.Name) {
                    case nameof(IVariableAnalyser): return typeof(VariableAnalyser);
                    case nameof(IIdentifierAnalyser): return typeof(IdentifierAnalyser);
                    case nameof(ITypeAnalyser): return typeof(TypeAnalyser);
                }

                throw new NotSupportedException($"{nameof(AnalyserTypeMapping)} does not support {type.Name}.");
            }
        }
    }
}
