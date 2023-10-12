using HB.NETF.Common.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Models {
    public struct IdentifierResult : IEquatable<IdentifierResult>, IComparable<IdentifierResult> {
        public IdentifierNameSyntax Origin { get; set; }
        public Location OriginLocation { get; set; }
        public IdentifierClassification OriginClassification { get; set; }
        public SyntaxNode Value { get; set; }
        public Location ValueLocation { get; set; }
        public IdentifierClassification ValueClassification { get; set; }

        public IdentifierResult(IdentifierNameSyntax origin, IdentifierClassification originClassification, SyntaxNode value, IdentifierClassification valueClassification) {
            this.Origin = origin;
            this.OriginLocation = origin.GetLocation();
            this.OriginClassification = originClassification;
            this.Value = value;
            this.ValueLocation = value.GetLocation();
            this.ValueClassification = valueClassification;
        }

        public static bool operator ==(IdentifierResult left, IdentifierResult right) {
            return left.Equals(right);
        }

        public static bool operator !=(IdentifierResult left, IdentifierResult right) {
            return !(left == right);
        }

        public override int GetHashCode() {
            int hashCode = -320094465;
            hashCode = hashCode * -1521134295 + ValueLocation.SourceSpan.GetHashCode();
            hashCode = hashCode * -1521134295 + ValueLocation.SourceTree.FilePath.GetHashCode();
            hashCode = hashCode * -1521134295 + OriginLocation.SourceSpan.GetHashCode();
            hashCode = hashCode * -1521134295 + OriginLocation.SourceTree.FilePath.GetHashCode();
            return hashCode;
        }

        public bool Equals(IdentifierResult other) {
            return this.OriginLocation.IsInSource
                && other.OriginLocation.IsInSource
                && this.OriginLocation.SourceTree.FilePath == other.OriginLocation.SourceTree.FilePath
                && this.OriginLocation.SourceSpan == other.OriginLocation.SourceSpan
                && other.ValueLocation.IsInSource
                && this.ValueLocation.IsInSource
                && this.ValueLocation.SourceTree.FilePath == other.ValueLocation.SourceTree.FilePath
                && this.ValueLocation.SourceSpan == other.ValueLocation.SourceSpan;
        }

        public override bool Equals(object obj) {
            return obj is IdentifierResult result && Equals(result);
        }

        public int CompareTo(IdentifierResult other) {
            return this.ValueLocation.SourceSpan.CompareTo(other.ValueLocation.SourceSpan);
        }

        public static IdentifierClassification MapSymbolKind(SymbolKind symbolKind) {
            switch (symbolKind) {
                case SymbolKind.Local: return IdentifierClassification.Local;
                case SymbolKind.Parameter: return IdentifierClassification.Parameter;
                case SymbolKind.Field: return IdentifierClassification.Field;
                case SymbolKind.Property: return IdentifierClassification.Property;
            }

            throw new InternalException("Invalid symbol, only [Local, Parameter, Field, Property] are supported.");
        }

        public static SymbolKind MapIdentifierClassification(IdentifierClassification classification) {
            switch (classification) {
                case IdentifierClassification.Local: return SymbolKind.Local;
                case IdentifierClassification.Field: return SymbolKind.Field;
                case IdentifierClassification.Parameter: return SymbolKind.Parameter;
                case IdentifierClassification.Property: return SymbolKind.Property;
            }

            throw new NotSupportedException($"{classification} not supported");
        }
    }

    public enum IdentifierClassification {
        Local,
        Parameter,
        Field,
        Property
    }
}
