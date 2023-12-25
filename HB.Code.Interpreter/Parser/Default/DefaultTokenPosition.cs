using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Parser.Default;
public class DefaultTokenPosition<TToken, TSyntaxKind> : ITokenPosition<TToken> {
    public DefaultTokenPosition<TToken, TSyntaxKind>? Previous { get; set; }
    public DefaultTokenPosition<TToken, TSyntaxKind>? Next { get; set; }
    public int Index { get; init; }
    public TToken Value { get; set; }

    public ImmutableArray<TSyntaxKind> PossibleNextTokens { get; set; }
    public ImmutableArray<TSyntaxKind> PossiblePreviousTokens { get; set; }

    public DefaultTokenPosition(TToken value, int index) {
        this.Value = value;
        this.Index = index;
    }
}
