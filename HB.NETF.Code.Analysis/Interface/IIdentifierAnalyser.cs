using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace HB.NETF.Code.Analysis.Interface {
    public interface IIdentifierAnalyser : ICodeAnalyser<ImmutableArray<IdentifierResult>> {
        ImmutableArray<IdentifierResult> FindIdentifiersFromSnapshot(SyntaxNode syntaxNode);
    }
}
