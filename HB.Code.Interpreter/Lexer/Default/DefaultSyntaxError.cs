using HB.Code.Interpreter.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Lexer.Default;
public class DefaultSyntaxError(int line, TextSpan fullSpan, TextSpan lineSpan, string affected) : ISyntaxError {
    public int Line { get; } = line;
    public TextSpan FullSpan { get; } = fullSpan;
    public TextSpan LineSpan { get; } = lineSpan;
    public string Affected { get; } = affected;
}
