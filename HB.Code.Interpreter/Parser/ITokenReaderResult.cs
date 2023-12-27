using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Parser;
public interface ITokenReaderResult<TToken> {
    public ITokenReaderResult<TToken> PeekNextToken(Func<int, TToken?> tokenGetter);
}
