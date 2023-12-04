using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace HB.NETF.Code.Analysis.Analyser {
    public abstract class AnalyserBase {
        protected Solution Solution { get; private set; }
        protected Project Project { get; private set; }
        protected IImmutableSet<Document> Documents { get; private set; }
        protected SemanticModel SemanticModel { get; private set; }
        protected SyntaxTree SyntaxTree { get; private set; }

        public virtual void Initialize(Solution solution, Project project, SemanticModel semanticModel) {
            this.Initialize(solution, project, project.Documents.ToImmutableHashSet(), semanticModel);
        }

        public void Initialize(Solution solution, Project project, IImmutableSet<Document> documents, SemanticModel semanticModel) {
            this.Solution = solution;
            this.Project = project;
            this.Documents = documents;
            this.SemanticModel = semanticModel;
            this.SyntaxTree = semanticModel.SyntaxTree;
        }
    }
}
