using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Syntax;
public interface ISyntaxTrivia {
    public bool Leading { get; }
    public TextSpan FullSpan { get; }
}
