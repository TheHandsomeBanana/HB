namespace HB.Code.Interpreter.Location;
public interface IPositionHandler<TPosition> where TPosition : IPosition {
    public TPosition CurrentPosition { get; }
    public void MoveNext(int steps);
}
