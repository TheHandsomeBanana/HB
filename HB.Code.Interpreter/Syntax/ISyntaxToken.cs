using HB.Code.Interpreter.Location;

namespace HB.Code.Interpreter.Syntax;
public interface ISyntaxToken {
    public string Value { get; }
    public TextSpan Span { get; }
}