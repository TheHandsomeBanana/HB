using HB.Code.Interpreter.Evaluator;
using HB.Code.Interpreter.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Analyser;
public interface ISemanticEvaluator<TSyntaxTree, TError> where TSyntaxTree : ISyntaxTree where TError : ISemanticError {
    public ImmutableArray<TError> Evaluate(TSyntaxTree syntaxTree, string content);
}
