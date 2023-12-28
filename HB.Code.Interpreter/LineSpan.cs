using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter;
public readonly struct LineSpan : IEquatable<LineSpan> {
    public int Line { get; }
    public TextSpan Span { get; }

    public LineSpan(int line, TextSpan span) {
        Line = line;
        Span = span;
    }

    public LineSpan(int line, int start, int length) {
        Line = line;
        Span = new TextSpan(start, length);
    }

    public bool Equals(LineSpan other) {
       return this.Line == other.Line && this.Span == other.Span;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) {
        return obj is LineSpan lineSpan && Equals(lineSpan);
    }

    public override int GetHashCode() => HashCode.Combine(Line, Span);

    public override string? ToString() {
        return $"{Line} {Span}";
    }

    public static bool operator ==(LineSpan left, LineSpan right) {
        return left.Equals(right);
    }

    public static bool operator !=(LineSpan left, LineSpan right) {
        return !(left == right);
    }
}
