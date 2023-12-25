using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Parser;
public interface ITokenPosition<TToken> {
    public int Index { get; }
    public TToken Value { get; }
}
