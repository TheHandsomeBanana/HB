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
            documents = project.Documents.ToImmutableHashSet();
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

        public async Task<IdentifierResult?> GetFromIdentifier(IdentifierNameSyntax identifier) {
            List<IdentifierResultValue> resultValues = new List<IdentifierResultValue>();

            ISymbol identifierSymbol = SemanticModel.GetSymbolInfo(identifier).Symbol;
            if (identifierSymbol == null)
                return null;

            SyntaxReference declarationReference = identifierSymbol.DeclaringSyntaxReferences.FirstOrDefault();
            if (declarationReference == null)
                return null;

            SyntaxNode declaration = await declarationReference.GetSyntaxAsync();

            IEnumerable<ReferencedSymbol> references = await SymbolFinder.FindReferencesAsync(identifierSymbol, solution, documents);
            IEnumerable<Location> referenceLocations = references.SelectMany(l => l.Locations.Select(s => s.Location));

            switch (declaration) {
                case ParameterSyntax parameter:
                    resultValues.AddRange(await ResolveParameter(parameter));
                    break;
                case PropertyDeclarationSyntax propertyDeclaration:
                    resultValues.AddRange(await ResolvePropertyDeclaration(propertyDeclaration));
                    break;
                case VariableDeclaratorSyntax variableDeclarator:
                    resultValues.AddRange(await ResolveVariableDeclarator(variableDeclarator));
                    break;
            }

            return new IdentifierResult(identifier, IdentifierResult.MapSymbolKind(identifierSymbol.Kind), resultValues.ToImmutableArray());
        }

        public async Task<IdentifierResult?> Run(SyntaxNode syntaxNode) {
            if (!(syntaxNode is IdentifierNameSyntax identifier))
                identifier = await FindFirstIdentifierFromSnapshot(syntaxNode);

            return await GetFromIdentifier(identifier);
        }

        private async Task<IdentifierResultValue[]> ResolveParameter(ParameterSyntax parameter) {
            throw new NotImplementedException();
        }

        private async Task<IdentifierResultValue[]> ResolvePropertyDeclaration(PropertyDeclarationSyntax propertyDeclaration) {
            throw new NotImplementedException();
        }

        private async Task<IdentifierResultValue[]> ResolveVariableDeclarator(VariableDeclaratorSyntax variableDeclarator) {
            throw new NotImplementedException();
        }
    }
}
