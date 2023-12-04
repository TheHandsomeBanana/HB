using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace HB.NETF.Code.Analysis.Models {
    public readonly struct VariableResult : IEquatable<VariableResult> {
        public SyntaxNode Origin { get; }
        public Location Location { get; }
        public ImmutableArray<VariableResultValue> Values { get; }

        public VariableResult(SyntaxNode origin, ImmutableArray<VariableResultValue> values) {
            this.Origin = origin;
            this.Location = origin.GetLocation();
            this.Values = values;
        }

        public static bool operator ==(VariableResult left, VariableResult right) {
            return left.Equals(right);
        }

        public static bool operator !=(VariableResult left, VariableResult right) {
            return !(left == right);
        }

        public override int GetHashCode() {
            int hashCode = -320094465;
            foreach (VariableResultValue identifierResultValue in Values)
                hashCode = hashCode * -1521134295 + identifierResultValue.GetHashCode();

            hashCode = hashCode * -1521134295 + Location.SourceSpan.GetHashCode();
            hashCode = hashCode * -1521134295 + Location.SourceTree.FilePath.GetHashCode();
            return hashCode;
        }

        public bool Equals(VariableResult other) {
            return this.Location.IsInSource
                && other.Location.IsInSource
                && this.Location.SourceTree.FilePath == other.Location.SourceTree.FilePath
                && this.Location.SourceSpan == other.Location.SourceSpan
                && other.Values.SequenceEqual(other.Values);
        }

        public override bool Equals(object obj) {
            return obj is VariableResult result && Equals(result);
        }

        public override string ToString() {
            string res = $"Origin: {Origin.ToFullString()} ({Location.SourceTree.FilePath} [{Location.SourceSpan}])";
            foreach (VariableResultValue identifierResultValue in Values)
                res += $"\nValue: {identifierResultValue}";

            return res;
        }

        public static readonly VariableResult Default = new VariableResult();
    }


    public readonly struct VariableResultValue : IEquatable<VariableResultValue>, IComparable<VariableResultValue> {
        public SyntaxNode Value { get; }
        public Location Location { get; }

        public VariableResultValue(SyntaxNode value) {
            this.Value = value;
            this.Location = value.GetLocation();
        }

        public static bool operator ==(VariableResultValue left, VariableResultValue right) {
            return left.Equals(right);
        }

        public static bool operator !=(VariableResultValue left, VariableResultValue right) {
            return !(left == right);
        }

        public override bool Equals(object obj) {
            return obj is VariableResultValue result && Equals(result);
        }

        public override int GetHashCode() {
            int hashCode = -320094465;
            hashCode = hashCode * -1521134295 + Location.SourceSpan.GetHashCode();
            hashCode = hashCode * -1521134295 + Location.SourceTree.FilePath.GetHashCode();
            return hashCode;
        }

        public override string ToString() {
            return $"{Value} ({Location.SourceTree.FilePath} [{Location.SourceSpan}])";
        }


        public bool Equals(VariableResultValue other) {
            return this.Location.IsInSource
                && other.Location.IsInSource
                && this.Location.SourceTree.FilePath == other.Location.SourceTree.FilePath
                && this.Location.SourceSpan == other.Location.SourceSpan;
        }

        public int CompareTo(VariableResultValue other) {
            return this.Location.SourceSpan.CompareTo(other.Location.SourceSpan);
        }

        public static readonly VariableResult Default = new VariableResult();
    }
}



