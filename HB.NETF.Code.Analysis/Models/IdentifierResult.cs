using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Models {
    public struct IdentifierResult : IEquatable<IdentifierResult>, IComparable<IdentifierResult> {
        public IdentifierNameSyntax Value { get; set; }
        public Location Location { get; set; }
        public SymbolKind? Kind { get; set; }

        public IdentifierResult(IdentifierNameSyntax value, SymbolKind? kind) {
            this.Value = value;
            this.Location = value.GetLocation();
            this.Kind = kind;
        }

        public static bool operator ==(IdentifierResult left, IdentifierResult right) {
            return left.Equals(right);
        }

        public static bool operator !=(IdentifierResult left, IdentifierResult right) {
            return !(left == right);
        }

        public override bool Equals(object obj) {
            return obj is IdentifierResult result && Equals(result);
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


        public bool Equals(IdentifierResult other) {
            return this.Location.IsInSource
                && other.Location.IsInSource
                && this.Location.SourceTree.FilePath == other.Location.SourceTree.FilePath
                && this.Location.SourceSpan == other.Location.SourceSpan;
        }

        public int CompareTo(IdentifierResult other) {
            return this.Location.SourceSpan.CompareTo(other.Location.SourceSpan);
        }

        public static readonly IdentifierResult Default = new IdentifierResult();
    }
}
