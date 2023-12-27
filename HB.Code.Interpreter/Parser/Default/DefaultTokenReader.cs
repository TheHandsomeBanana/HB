using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Parser.Default;
public class DefaultTokenReader<TToken, TSyntaxKind> : ITokenReader<TToken> {
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

    public ITokenReaderResult<TToken> PeekNextToken() {
        TToken? foundToken = GetTokenAt(CurrentIndex + 1);
        int tokenIndex = foundToken is null ? -1 : CurrentIndex + 1;
        return new DefaultTokenReaderResult<TToken>(foundToken, tokenIndex, 0);
    }

    private TToken? GetTokenAt(int index) => CanMoveNext() ? tokens[index] : default;

    public void FinishPeek(ITokenReaderResult<TToken> tokenResult) {
        for(int i = 0; i < ((DefaultTokenReaderResult<TToken>)tokenResult).PeekCounter; i++) {
            if (!CanMoveNext())
                break;

            MoveNext();
        }
    }
}
