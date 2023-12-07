using HB.Code.Interpreter.Syntax;
using System.Collections.Immutable;

namespace HB.Code.Interpreter.Parser;

public interface IParser<TSyntaxTree, TToken> where TSyntaxTree : ISyntaxTree where TToken : ISyntaxToken {
    public ParserFunctionList ParserFunctions { get; }
    public void AddParserFunction<T>(IParserFunction<T> function);
    TSyntaxTree Parse(ImmutableArray<TToken> tokens);
}
