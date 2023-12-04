using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Interface {
    public interface IVariableAnalyser : ICodeAnalyser<VariableResult?> {
        Task<IdentifierNameSyntax[]> GetPossibleIdentifiersFromSnapshot(SyntaxNode node);
    }
}
