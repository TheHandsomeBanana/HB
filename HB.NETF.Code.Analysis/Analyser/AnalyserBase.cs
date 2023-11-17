using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Analyser {
    public abstract class AnalyserBase {
        protected Solution Solution { get; }
        protected Project Project { get; }
        protected IImmutableSet<Document> Documents { get; }
        protected SemanticModel SemanticModel { get; }
        protected SyntaxTree SyntaxTree { get; }

        public AnalyserBase(Solution solution, Project project, SemanticModel semanticModel) {
            this.Solution = solution;
            this.Project = project;
            this.Documents = project.Documents.ToImmutableHashSet();
            this.SemanticModel = semanticModel;
            this.SyntaxTree = semanticModel.SyntaxTree;
        }
    }
}
