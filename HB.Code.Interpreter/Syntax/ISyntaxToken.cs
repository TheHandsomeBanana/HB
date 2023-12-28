namespace HB.Code.Interpreter.Syntax;
public interface ISyntaxToken {
    public string Value { get; }
    public TextSpan FullSpan { get; }
    public LineSpan LineSpan { get; }
}