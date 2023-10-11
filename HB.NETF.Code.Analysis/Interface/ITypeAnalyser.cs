using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Interface {
    public interface ITypeAnalyser {
        SemanticModel SemanticModel { get; }
        Type[] TypeFilter { get; }
        Task<SearchType?> GetFirstFromSnapshotNode(SyntaxNode syntaxNode);
        Task<SearchType[]> GetAll(SyntaxTree syntaxTree);
    }

    public interface ITypeAnalyser<out T> {
        SemanticModel SemanticModel { get; }
        Type TypeFilter { get; }

        Task<SearchType?> GetFromTriggeredNode(SyntaxNode syntaxNode);
        Task<SearchType[]> GetAll(SyntaxTree syntaxTree);
    }
}
