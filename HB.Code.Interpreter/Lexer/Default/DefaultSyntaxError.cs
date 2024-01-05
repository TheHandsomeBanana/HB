using HB.Code.Interpreter.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Lexer.Default;
public class DefaultSyntaxError(TextSpan fullSpan, LineSpan? lineSpan = null, string? affected = null, string? message = null) : ISyntaxError {
    public TextSpan FullSpan { get; } = fullSpan;
    public LineSpan? LineSpan { get; } = lineSpan;
    public string? Affected { get; private set; } = affected;
    public string? Message { get; private set; } = message;

    public override string ToString() {
        return LineSpan != null
            ? $"Syntax error at line {LineSpan} ({FullSpan}): {Affected}. {Message}"
            : $"Syntax error at {FullSpan}: {Affected}. {Message}";
    }

    public void SetAffected(string fullContent) {
        Affected = fullContent.Substring(FullSpan.Start, FullSpan.Length);
    }
}
