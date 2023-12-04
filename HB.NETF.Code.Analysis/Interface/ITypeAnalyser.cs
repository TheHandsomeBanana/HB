using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Interface {
    public interface ITypeAnalyser : ICodeAnalyser<TypeResult?> {
        Type[] TypeFilter { get; set; }
        Task<TypeResult?> GetFirstFromSnapshot(SyntaxNode syntaxNode);
        Task<TypeResult[]> GetAll(SyntaxTree syntaxTree);
    }

    public interface ITypeAnalyser<out T> : ICodeAnalyser<TypeResult?> {
        Task<TypeResult?> GetFirstFromSnapshot(SyntaxNode syntaxNode);
        Task<TypeResult[]> GetAll(SyntaxTree syntaxTree);
    }
}
