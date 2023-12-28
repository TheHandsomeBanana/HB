using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Evaluator;
public interface ISemanticError {
    public TextSpan Location { get; }
    public string Affected { get; }
    public string Message { get; }
}
