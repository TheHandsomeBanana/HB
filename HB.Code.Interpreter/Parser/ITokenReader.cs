﻿using HB.Code.Interpreter.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Parser;
public interface ITokenReader<TToken> {
    public void Init(ImmutableArray<TToken> tokens);
    public void MoveNext();
    public bool CanMoveNext();
    public ITokenReaderResult<TToken> PeekNextToken();
    public void FinishPeek(ITokenReaderResult<TToken> tokenResult);
}
