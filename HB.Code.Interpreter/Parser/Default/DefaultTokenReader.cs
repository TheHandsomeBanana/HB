using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Parser.Default;
public class DefaultTokenReader<TToken, TSyntaxKind> : ITokenReader<TToken> {
    public DefaultTokenPosition<TToken, TSyntaxKind>? RootTokenPosition { get; private set; }
    public void Init(ImmutableArray<TToken> tokens) {
        RootTokenPosition = new DefaultTokenPosition<TToken, TSyntaxKind>(tokens[0], 0);
        DefaultTokenPosition<TToken, TSyntaxKind> currentPosition = RootTokenPosition;

        // Build double linked list
        for (int i = 1; i < tokens.Length; i++) {
            DefaultTokenPosition<TToken, TSyntaxKind> temp = currentPosition;
            currentPosition.Next = new DefaultTokenPosition<TToken, TSyntaxKind>(tokens[i], i);
            currentPosition = currentPosition.Next;
            currentPosition.Previous = temp;
        }
    }
}
