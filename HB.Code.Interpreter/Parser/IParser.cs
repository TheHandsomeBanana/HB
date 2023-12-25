using HB.Code.Interpreter.Syntax;
using System.Collections.Immutable;

namespace HB.Code.Interpreter.Parser;

public interface IParser<TSyntaxTree, TToken, TError> where TSyntaxTree : ISyntaxTree where TToken : ISyntaxToken where TError : ISyntaxError {
    TSyntaxTree Parse(ImmutableArray<TToken> tokens);
    ImmutableArray<TError> GetSyntaxErrors();
}
