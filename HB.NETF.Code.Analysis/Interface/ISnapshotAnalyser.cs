using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Interface {
    public interface ISnapshotAnalyser<TResult> {
        Task<TResult> ExecuteAsync(SyntaxNode node);
    }
}
