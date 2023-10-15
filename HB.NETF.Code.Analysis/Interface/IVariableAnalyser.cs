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
    public interface IVariableAnalyser : ICodeAnalyser<VariableResult?> {
        Task<IdentifierNameSyntax[]> GetPossibleIdentifiersFromSnapshot(SyntaxNode node);
    }
}
