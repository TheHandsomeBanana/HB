using HB.Code.Interpreter.Syntax;
using System.Collections.Immutable;

namespace HB.Code.Interpreter.Lexer;
public interface ILexer<T> where T : ISyntaxToken {
    public ImmutableArray<T> Lex(string input); // T is symbol type
}
