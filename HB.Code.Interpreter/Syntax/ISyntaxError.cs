using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Syntax;
public interface ISyntaxError {
    public TextSpan FullSpan { get; }
    public string Affected { get; }
}
