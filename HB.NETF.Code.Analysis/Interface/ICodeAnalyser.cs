using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Interface {
    public interface ICodeAnalyser {
        void Initialize(Solution solution, Project project, SemanticModel semanticModel);
    }

    public interface ICodeAnalyser<TResult> : ICodeAnalyser {
        /// <summary>
        /// Runs analysis for only 1 file given through provided <see cref="SyntaxNode"/>.<br></br>
        /// Analyser specific -> returns <typeparamref name="TResult"/> closest to snapshot in most cases. 
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <returns></returns>
        Task<TResult> Run(SyntaxNode syntaxNode);
    }
}
