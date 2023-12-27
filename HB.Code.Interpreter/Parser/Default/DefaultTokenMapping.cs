using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Parser.Default;
public class DefaultTokenMapping<TTokenKind, TSyntaxKind> : ITokenMapping<TTokenKind, TSyntaxKind> where TTokenKind : notnull {
    private readonly Dictionary<TTokenKind, TSyntaxKind[]> succeedingKinds = [];
    private readonly Dictionary<TTokenKind, TSyntaxKind[]> predecessingKinds = [];
    public IReadOnlyDictionary<TTokenKind, TSyntaxKind[]> SucceedingKinds => succeedingKinds;
    public IReadOnlyDictionary<TTokenKind, TSyntaxKind[]> PredecessingKinds => predecessingKinds;

    public void AddPredecessingMap(TTokenKind tokenKind, params TSyntaxKind[] possibleKinds) {
        predecessingKinds[tokenKind] = possibleKinds;
    }

    public void AddSucceedingMap(TTokenKind tokenKind, params TSyntaxKind[] possibleKinds) {
        succeedingKinds[tokenKind] = possibleKinds;
    }


}
