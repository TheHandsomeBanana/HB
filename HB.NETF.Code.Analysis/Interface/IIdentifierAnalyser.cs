﻿using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Interface {
    public interface IIdentifierAnalyser : ICodeAnalyser<ImmutableArray<IdentifierResult>> {
        ImmutableArray<IdentifierResult> FindIdentifiersFromSnapshot(SyntaxNode syntaxNode);
    }
}
