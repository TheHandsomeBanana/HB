using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Analyser {
    public class IdentifierAnalyser : AnalyserBase, IIdentifierAnalyser {

        public ImmutableArray<IdentifierResult> FindIdentifiersFromSnapshot(SyntaxNode syntaxNode) {
            while (syntaxNode != null && !(syntaxNode.Parent is BlockSyntax) && !(syntaxNode.Parent is TypeDeclarationSyntax)) {
                syntaxNode = syntaxNode.Parent;
            }
            if (syntaxNode is null)
                return ImmutableArray<IdentifierResult>.Empty;

            return syntaxNode.DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .Select(e => new IdentifierResult(e, SemanticModel.GetSymbolInfo(e).Symbol?.Kind))
                .ToImmutableArray();
        }

        public async Task<ImmutableArray<IdentifierResult>> Run(SyntaxNode syntaxNode) => await Task.Run(() => FindIdentifiersFromSnapshot(syntaxNode));

    }
}
