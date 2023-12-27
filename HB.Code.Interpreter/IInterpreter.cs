using System.Collections.Immutable;
using System.Globalization;

namespace HB.Code.Interpreter;

public interface IInterpreter {
    public void Run(string input);
    public void RunFromFile(string filePath);
    public ImmutableArray<string> GetErrors();
}
