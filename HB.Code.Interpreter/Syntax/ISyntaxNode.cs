using HB.Code.Interpreter.Location;

namespace HB.Code.Interpreter.Syntax;
public interface ISyntaxNode {
    public TextSpan Span { get; }
    public ISyntaxNode? Parent { get; }
    public IReadOnlyList<ISyntaxNode> ChildNodes { get; }
    public IReadOnlyList<ISyntaxToken> ChildTokens { get; }
    public void AddChildNode(ISyntaxNode node);
    public void AddChildToken(ISyntaxToken token);
}

public interface ISyntaxNode<TNode, TToken> : ISyntaxNode where TNode : ISyntaxNode where TToken : ISyntaxToken {
    new public TNode? Parent { get; }
    new public IReadOnlyList<TNode> ChildNodes { get; }
    new public IReadOnlyList<TToken> ChildTokens { get; }
    public void AddChildNode(TNode node);
    public void AddChildToken(TToken token);
}