using HB.NETF.Code.Analysis.Models;
using HB.NETF.Common.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Models {
    public readonly struct IdentifierResult : IEquatable<IdentifierResult> {
        public IdentifierNameSyntax Origin { get; }
        public Location Location { get; }
        public IdentifierClassification Classification { get; }
        public ImmutableArray<IdentifierResultValue> Values { get; }

        public IdentifierResult(IdentifierNameSyntax origin, IdentifierClassification originClassification, ImmutableArray<IdentifierResultValue> values) {
            this.Origin = origin;
            this.Location = origin.GetLocation();
            this.Classification = originClassification;
            this.Values = values;
        }

        public static bool operator ==(IdentifierResult left, IdentifierResult right) {
            return left.Equals(right);
        }

        public static bool operator !=(IdentifierResult left, IdentifierResult right) {
            return !(left == right);
        }

        public override int GetHashCode() {
            int hashCode = -320094465;
            foreach (IdentifierResultValue identifierResultValue in Values)
                hashCode = hashCode * -1521134295 + identifierResultValue.GetHashCode();

            hashCode = hashCode * -1521134295 + Location.SourceSpan.GetHashCode();
            hashCode = hashCode * -1521134295 + Location.SourceTree.FilePath.GetHashCode();
            return hashCode;
        }

        public bool Equals(IdentifierResult other) {
            return this.Location.IsInSource
                && other.Location.IsInSource
                && this.Location.SourceTree.FilePath == other.Location.SourceTree.FilePath
                && this.Location.SourceSpan == other.Location.SourceSpan
                && other.Values.SequenceEqual(other.Values);
        }

        public override bool Equals(object obj) {
            return obj is IdentifierResult result && Equals(result);
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

        public static readonly IdentifierResult Default = new IdentifierResult();
    }


    public readonly struct IdentifierResultValue : IEquatable<IdentifierResultValue>, IComparable<IdentifierResultValue> {
        public SyntaxNode Value { get; }
        public Location Location { get; }
        public IdentifierClassification Classification { get; }

        public IdentifierResultValue(SyntaxNode value, IdentifierClassification classification) {
            this.Value = value;
            this.Location = value.GetLocation();
            this.Classification = classification;
        }

        public static bool operator ==(IdentifierResultValue left, IdentifierResultValue right) {
            return left.Equals(right);
        }

        public static bool operator !=(IdentifierResultValue left, IdentifierResultValue right) {
            return !(left == right);
        }

        public override bool Equals(object obj) {
            return obj is IdentifierResultValue result && Equals(result);
        }

        public override int GetHashCode() {
            int hashCode = -320094465;
            hashCode = hashCode * -1521134295 + Location.SourceSpan.GetHashCode();
            hashCode = hashCode * -1521134295 + Location.SourceTree.FilePath.GetHashCode();
            return hashCode;
        }

        public bool Equals(IdentifierResultValue other) {
            return this.Location.IsInSource
                && other.Location.IsInSource
                && this.Location.SourceTree.FilePath == other.Location.SourceTree.FilePath
                && this.Location.SourceSpan == other.Location.SourceSpan;
        }

        public int CompareTo(IdentifierResultValue other) {
            return this.Location.SourceSpan.CompareTo(other.Location.SourceSpan);
        }

        public static readonly IdentifierResult Default = new IdentifierResult();
    }

    public enum IdentifierClassification {
        Undefined,
        Local,
        Parameter,
        Field,
        Property
    }
}



