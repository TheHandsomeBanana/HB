namespace HB.Code.Interpreter.Syntax;
public interface ISyntaxTree { // Flag for constraint
}

public interface ISyntaxTree<TNode> : ISyntaxTree where TNode : ISyntaxNode {
    public TNode? Root { get; }
    public string? FilePath { get; }
    public TNode[] GetNodes();

}
