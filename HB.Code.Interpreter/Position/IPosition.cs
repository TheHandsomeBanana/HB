namespace HB.Code.Interpreter.Location;
public interface IPosition {
    public int Index { get; }
    public char GetValue(string content);
}
