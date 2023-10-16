using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Interface {
    public interface ICodeAnalyser {
        /// <summary>
        /// Runs analysis for only 1 file given through provided <see cref="SyntaxNode"/>.<br></br>
        /// Analyser specific -> returns <see cref="object"/> closest to snapshot in most cases. 
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <returns></returns>
        Task<object> Run(SyntaxNode syntaxNode);
    }

    public interface ICodeAnalyser<TResult> : ICodeAnalyser {
        /// <summary>
        /// Runs analysis for only 1 file given through provided <see cref="SyntaxNode"/>.<br></br>
        /// Analyser specific -> returns <typeparamref name="TResult"/> closest to snapshot in most cases. 
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <returns></returns>
        new Task<TResult> Run(SyntaxNode syntaxNode);
    }
}
