using HB.NETF.Code.Analysis.Analyser;
using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HB.NETF.Code.Analysis.Factory {
    public class AnalyserFactory : IAnalyserFactory {
        private readonly Dictionary<AnalyserFactoryKey, ICodeAnalyser> analyserContainer = new Dictionary<AnalyserFactoryKey, ICodeAnalyser>();
        public IReadOnlyDictionary<AnalyserFactoryKey, ICodeAnalyser> AnalyserContainer => analyserContainer;
        public SemanticModelCache SemanticModelCache { get; set; }



        public TAnalyser GetOrCreateAnalyser<TAnalyser>(Solution solution, Project project, SemanticModel semanticModel) where TAnalyser : ICodeAnalyser {
            try {
                TAnalyser analyser = Activator.CreateInstance<TAnalyser>();
                analyser.Initialize(solution, project, semanticModel);
                return analyser;
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

            ITypeAnalyser analyser = new TypeAnalyser();
            analyser.Initialize(solution, project, semanticModel);
            analyser.TypeFilter = typeFilter;

            analyserContainer.Add(key, analyser);
            return analyser;
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

        private ICodeAnalyser GetOrCreateAnalyser(Type analyserType, Solution solution, Project project, SemanticModel semanticModel) {
            if (analyserType.IsInterface)
                analyserType = AnalyserTypeMapping.Get(analyserType);

            AnalyserFactoryKey key = new AnalyserFactoryKey(analyserType, semanticModel.SyntaxTree.FilePath);

            if (AnalyserContainer.ContainsKey(key))
                return analyserContainer[key];

            ICodeAnalyser analyser = Activator.CreateInstance(analyserType) as ICodeAnalyser;
            analyser.Initialize(solution, project, semanticModel);
            analyserContainer.Add(key, analyser);
            return analyser;
        }

        private ICodeAnalyser GetAnalyser(AnalyserFactoryKey key) {
            if (!analyserContainer.ContainsKey(key))
                return null;

            return analyserContainer[key];
        }

        private IEnumerable<ICodeAnalyser> GetAnalysers(Type analyserType) {
            return analyserContainer.Where(e => e.Key.AnalyserType == analyserType).Select(e => e.Value);
        }

        /// <summary>
        /// Helper class, reflection type loading takes way too much time
        /// </summary>
        static class AnalyserTypeMapping {
            public static Type Get(Type type) {
                switch (type.Name) {
                    case nameof(IVariableAnalyser): return typeof(VariableAnalyser);
                    case nameof(IIdentifierAnalyser): return typeof(IdentifierAnalyser);
                    case nameof(ITypeAnalyser): return typeof(TypeAnalyser);
                }

                throw new NotSupportedException($"{nameof(AnalyserTypeMapping)} does not support {type.Name}.");
            }
        }
    }
}
