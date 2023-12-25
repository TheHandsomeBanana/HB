namespace HB.Code.Interpreter.Lexer;
public interface IPositionHandler<TPosition> where TPosition : IPosition
{
    public string Content { get; }
    public TPosition CurrentPosition { get; }
    public void Init(string content);
    public void MoveNext(int steps);
    public void MoveNextWhile(int steps, Predicate<TPosition> predicate);
    public void Skip(int steps);
    public void Reset();
}
