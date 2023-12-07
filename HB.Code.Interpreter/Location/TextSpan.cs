namespace HB.Code.Interpreter.Location;

public readonly struct TextSpan : IEquatable<TextSpan>, IComparable<TextSpan> {
    public TextSpan(int start, int length) {
        ArgumentOutOfRangeException.ThrowIfNegative(start);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(start, start + length);

        Start = start;
        Length = length;
    }

    public int Start { get; }
    public int End => Start + Length;
    public int Length { get; }
    public bool IsEmpty => this.Length == 0;
    public bool Contains(int position) {
        return unchecked((uint)(position - Start) < (uint)Length);
    }
    public bool Contains(TextSpan span) {
        return span.Start >= Start && span.End <= this.End;
    }

    public bool OverlapsWith(TextSpan span) {
        int overlapStart = Math.Max(Start, span.Start);
        int overlapEnd = Math.Min(this.End, span.End);

        return overlapStart < overlapEnd;
    }

    public bool IntersectsWith(TextSpan span) {
        return span.Start <= this.End && span.End >= Start;
    }

    public bool IntersectsWith(int position) {
        return unchecked((uint)(position - Start) <= (uint)Length);
    }

    public static bool operator ==(TextSpan left, TextSpan right) {
        return left.Equals(right);
    }

    public static bool operator !=(TextSpan left, TextSpan right) {
        return !left.Equals(right);
    }

    public bool Equals(TextSpan other) {
        return Start == other.Start && Length == other.Length;
    }

    public override bool Equals(object? obj)
        => obj is TextSpan span && Equals(span);

    public override int GetHashCode() {
        return HashCode.Combine(Start, Length);
    }

    public override string ToString() {
        return $"[{Start}..{End})";
    }

    public int CompareTo(TextSpan other) {
        var diff = Start - other.Start;
        if (diff != 0) {
            return diff;
        }

        return Length - other.Length;
    }
}

