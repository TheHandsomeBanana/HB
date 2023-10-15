using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Interface {
    public interface ICodeAnalyser {
        /// <summary>
        /// Analyser specific. Returns <see cref="TResult"/> closest to snapshot in most cases. 
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <returns></returns>
        Task<object> Run(SyntaxNode syntaxNode);
    }

    public interface ICodeAnalyser<TResult> : ICodeAnalyser {
        /// <summary>
        /// Analyser specific. Returns <see cref="TResult"/> closest to snapshot in most cases. 
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <returns></returns>
        new Task<TResult> Run(SyntaxNode syntaxNode);
    }
}
