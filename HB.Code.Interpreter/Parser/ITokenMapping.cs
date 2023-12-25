using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Parser;
public interface ITokenMapping<TToken, TSyntaxKind> {
    public void AddSucceedingMap(TToken tokenKind, TSyntaxKind[] possibleKinds);
    public void AddPredecessingMap(TToken tokenKind, TSyntaxKind[] possibleKinds);
}
