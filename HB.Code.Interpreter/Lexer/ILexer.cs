using HB.Code.Interpreter.Syntax;
using System.Collections.Immutable;

namespace HB.Code.Interpreter.Lexer;
public interface ILexer<TToken, TError> where TToken : ISyntaxToken where TError : ISyntaxError {
    public ImmutableArray<TToken> Lex(string input);
    public ImmutableArray<TError> GetSyntaxErrors();
}
