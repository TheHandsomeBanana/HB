using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Analyser {
    public class IdentifierAnalyser : IIdentifierAnalyser {
        private readonly Solution solution;
        private readonly IImmutableSet<Document> documents;
        public SemanticModel SemanticModel { get; }

        public IdentifierAnalyser(SemanticModel semanticModel, Solution solution, Project project) {
            this.SemanticModel = semanticModel;
            this.solution = solution;
            
        }

        public async Task<IdentifierNameSyntax[]> FindAllIdentifiersFromSnapshot(SyntaxNode syntaxNode) {
            throw new NotImplementedException();
        }

        public async Task<IdentifierNameSyntax[]> FindAllIdentifiersFromSnapshot(SyntaxNode syntaxNode, IdentifierClassification classification) {
            throw new NotImplementedException();
        }

        public async Task<IdentifierNameSyntax> FindFirstIdentifierFromSnapshot(SyntaxNode syntaxNode) {
            throw new NotImplementedException();
        }

        public async Task<IdentifierNameSyntax> FindFirstIdentifierFromSnapshot(SyntaxNode syntaxNode, IdentifierClassification classification) {
            throw new NotImplementedException();
        }

        public async Task<IdentifierResult[]> GetFromIdentifier(IdentifierNameSyntax identifier) {
            ISymbol identifierSymbol = SemanticModel.GetSymbolInfo(identifier).Symbol;

            if (identifierSymbol == null)
                return Array.Empty<IdentifierResult>();

            SyntaxReference foundDefinition = identifierSymbol.DeclaringSyntaxReferences.FirstOrDefault();
            if (foundDefinition == null)
                return Array.Empty<IdentifierResult>();

            SyntaxNode foundDefinitionSyntax = await foundDefinition.GetSyntaxAsync();

            IEnumerable<ReferencedSymbol> references = await SymbolFinder.FindReferencesAsync(identifierSymbol, solution, documents);
            IEnumerable<Location> referenceLocations = references.SelectMany(l => l.Locations.Select(s => s.Location));
        }

        public async Task<IdentifierResult[]> Run(SyntaxNode syntaxNode) {
            if (!(syntaxNode is IdentifierNameSyntax identifier))
                identifier = await FindFirstIdentifierFromSnapshot(syntaxNode);

            return await GetFromIdentifier(identifier);
        }
    }
}
