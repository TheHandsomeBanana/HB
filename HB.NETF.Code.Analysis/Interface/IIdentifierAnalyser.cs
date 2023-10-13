using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Interface {
    public interface IIdentifierAnalyser : ICodeAnalyser<IdentifierResult?> {
        Task<IdentifierResult?> GetFromIdentifier(IdentifierNameSyntax identifier);
        Task<IdentifierNameSyntax> FindFirstIdentifierFromSnapshot(SyntaxNode syntaxNode);
        Task<IdentifierNameSyntax[]> FindAllIdentifiersFromSnapshot(SyntaxNode syntaxNode);
        Task<IdentifierNameSyntax> FindFirstIdentifierFromSnapshot(SyntaxNode syntaxNode, IdentifierClassification classification);
        Task<IdentifierNameSyntax[]> FindAllIdentifiersFromSnapshot(SyntaxNode syntaxNode, IdentifierClassification classification);
    }
}
