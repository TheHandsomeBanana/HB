using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Models {
    public readonly struct SearchType {
        public SyntaxNode SyntaxNode { get; }
        public ITypeSymbol TypeInfo { get; }
        public Location Location { get; }

        public SearchType(SyntaxNode syntaxNode, ITypeSymbol typeInfo) {
            this.SyntaxNode = syntaxNode; 
            this.TypeInfo = typeInfo;
            this.Location = syntaxNode.GetLocation();
        }

        public static bool operator ==(SearchType left, SearchType right) {
            return left.Location.IsInSource
                && right.Location.IsInSource
                && left.Location.SourceTree.FilePath == right.Location.SourceTree.FilePath
                && left.Location.SourceSpan == right.Location.SourceSpan;
        }

        public static bool operator !=(SearchType left, SearchType right) {
            return !(left == right);
        }

        public override bool Equals(object obj) {
            return obj is SearchType type &&
                   EqualityComparer<SyntaxNode>.Default.Equals(SyntaxNode, type.SyntaxNode) &&
                   EqualityComparer<ITypeSymbol>.Default.Equals(TypeInfo, type.TypeInfo) &&
                   EqualityComparer<Location>.Default.Equals(Location, type.Location);
        }

        public override int GetHashCode() {
            int hashCode = 282772057;
            hashCode = hashCode * -1521134295 + EqualityComparer<SyntaxNode>.Default.GetHashCode(SyntaxNode);
            hashCode = hashCode * -1521134295 + EqualityComparer<ITypeSymbol>.Default.GetHashCode(TypeInfo);
            hashCode = hashCode * -1521134295 + EqualityComparer<Location>.Default.GetHashCode(Location);
            return hashCode;
        }
    }
}
