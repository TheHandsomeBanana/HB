namespace HB.Code.Interpreter.Syntax;
public interface ISyntaxTree {
    public ISyntaxNode? Root { get; }
    public string? FilePath { get; }
}

public interface ISyntaxTree<TNode> : ISyntaxTree where TNode : ISyntaxNode {
    new public TNode? Root { get; }
}
