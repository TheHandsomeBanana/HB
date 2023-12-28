using HB.Code.Interpreter.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Parser.Default;
public class DefaultTokenReader<TToken, TSyntaxKind> : ITokenReader<TToken> where TToken : ISyntaxToken {
    public int CurrentIndex { get; private set; }
    public TToken? CurrentToken => CurrentIndex < tokens.Length ? tokens[CurrentIndex] : default;
    private ImmutableArray<TToken> tokens = [];
    public void Init(ImmutableArray<TToken> tokens) {
        this.tokens = tokens;
        this.CurrentIndex = 0;
    }

    public void MoveNext() {
        if (CanMoveNext())
            CurrentIndex++;
    }

    public bool CanMoveNext() => CurrentIndex < tokens.Length - 1;

    public TToken? GetNextToken() {
        if (CanMoveNext())
            MoveNext();

        return CurrentToken;
    }

    private TToken? GetTokenAt(int index) => CanMoveNext() ? tokens[index] : default;

    public TextSpan? GetCurrentFullSpan() => CurrentToken?.FullSpan;
    public LineSpan? GetCurrentLineSpan() => CurrentToken?.LineSpan;

    public TToken GetLastValidToken() {
        int index = CurrentIndex;
        while (GetTokenAt(index) == null && index > 0)
            index--;

        return GetTokenAt(index)!;
    }
}
