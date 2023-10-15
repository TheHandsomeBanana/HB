using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Models {
    public readonly struct TypeResult : IEquatable<TypeResult>, IComparable<TypeResult> {
        public SyntaxNode SyntaxNode { get; }
        public ITypeSymbol TypeInfo { get; }
        public Location Location { get; }

        public TypeResult(SyntaxNode syntaxNode, ITypeSymbol typeInfo) {
            this.SyntaxNode = syntaxNode; 
            this.TypeInfo = typeInfo;
            this.Location = syntaxNode.GetLocation();
        }

        public static bool operator ==(TypeResult left, TypeResult right) {
            return left.Location.IsInSource
                && right.Location.IsInSource
                && left.Location.SourceTree.FilePath == right.Location.SourceTree.FilePath
                && left.Location.SourceSpan == right.Location.SourceSpan;
        }

        public static bool operator !=(TypeResult left, TypeResult right) {
            return !(left == right);
        }

        public override int GetHashCode() {
            int hashCode = -320094465;
            hashCode = hashCode * -1521134295 + Location.SourceSpan.GetHashCode();
            hashCode = hashCode * -1521134295 + Location.SourceTree.FilePath.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj) {
            return obj is TypeResult result && Equals(result);
        }

        public override string ToString() {
            return $"{SyntaxNode} ({Location.SourceTree.FilePath} [{Location.SourceSpan}])";
        }

        public bool Equals(TypeResult other) {
            return this.Location.IsInSource
                && other.Location.IsInSource
                && this.Location.SourceTree.FilePath == other.Location.SourceTree.FilePath
                && this.Location.SourceSpan == other.Location.SourceSpan;
        }

        public int CompareTo(TypeResult other) {
            return this.Location.SourceSpan.CompareTo(other.Location.SourceSpan);
        }


    }
}
