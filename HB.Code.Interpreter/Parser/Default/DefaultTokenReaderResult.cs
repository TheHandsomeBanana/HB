using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Parser.Default;
public class DefaultTokenReaderResult<TToken> : ITokenReaderResult<TToken> {
    public TToken? Value { get; }
    public int Index { get; }
    public int PeekCounter { get; }
    public DefaultTokenReaderResult(TToken? value, int index, int peekCounter) {
        Value = value;
        Index = index;
        PeekCounter = peekCounter + 1;
    }

    public ITokenReaderResult<TToken> PeekNextToken(Func<int, TToken?> tokenGetter) {
        TToken? foundToken = tokenGetter.Invoke(Index + 1);
        int tokenIndex = foundToken is null ? -1 : Index + 1;
        return new DefaultTokenReaderResult<TToken>(foundToken, tokenIndex, PeekCounter);
    }
}
