using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Code.Analysis.Models;
using HB.NETF.Code.Analysis.Resolver;
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
    public class VariableAnalyser : AnalyserBase, IVariableAnalyser {
        public VariableAnalyser(Solution solution, Project project, SemanticModel semanticModel) : base(solution, project, semanticModel) {
        }

        public async Task<VariableResult?> Run(SyntaxNode syntaxNode) {


            ImmutableArray<VariableResultValue> resultValues = await ResolveInternal(syntaxNode);
            return new VariableResult(syntaxNode, resultValues);
        }

        async Task<object> ICodeAnalyser.Run(SyntaxNode syntaxNode) => await Run(syntaxNode);

        public Task<IdentifierNameSyntax[]> GetPossibleIdentifiersFromSnapshot(SyntaxNode node) {
            throw new NotImplementedException();
        }


        #region Resolve
        private async Task<ImmutableArray<VariableResultValue>> ResolveInternal(SyntaxNode syntaxNode) {
            List<VariableResultValue> resultValues = new List<VariableResultValue>();
            switch(syntaxNode) {
                case IdentifierNameSyntax identifier:
                    resultValues.AddRange(await ResolveIdentifier(identifier)); 
                    break;
            }

            return resultValues.ToImmutableArray();
        }

        private async Task<ImmutableArray<VariableResultValue>> ResolveIdentifier(IdentifierNameSyntax identifier) {
            ISymbol identifierSymbol = SemanticModel.GetSymbolInfo(identifier).Symbol;
            if (identifierSymbol == null)
                return ImmutableArray<VariableResultValue>.Empty;

            SyntaxReference declarationReference = identifierSymbol.DeclaringSyntaxReferences.FirstOrDefault();
            if (declarationReference == null)
                return ImmutableArray<VariableResultValue>.Empty;

            SyntaxNode declaration = await declarationReference.GetSyntaxAsync();

            IEnumerable<Location> locations = await LocationResolver.FindReferenceLocations(identifierSymbol, Solution, Documents);

            switch (declaration) {
                case ParameterSyntax parameter:
                    return await ResolveParameter(parameter);
                case PropertyDeclarationSyntax propertyDeclaration:
                    return ResolvePropertyDeclaration(propertyDeclaration);
                case VariableDeclaratorSyntax variableDeclarator:
                    return ResolveVariableDeclarator(variableDeclarator);
            }

            return ImmutableArray<VariableResultValue>.Empty;
        }

        private async Task<ImmutableArray<VariableResultValue>> ResolveParameter(ParameterSyntax parameter) {
            ISymbol declaredSymbol = SemanticModel.GetDeclaredSymbol(parameter.Parent.Parent);
            if (declaredSymbol is null)
                return ImmutableArray<VariableResultValue>.Empty;

            IEnumerable<Location> locations = await LocationResolver.FindCallerLocations(declaredSymbol, Solution, Documents);


            return ImmutableArray<VariableResultValue>.Empty;
        }

        private ImmutableArray<VariableResultValue> ResolvePropertyDeclaration(PropertyDeclarationSyntax propertyDeclaration) {
            if(propertyDeclaration.Initializer != null) {
                return new[] { new VariableResultValue(propertyDeclaration.Initializer.Value) }.ToImmutableArray();
            }

            return ImmutableArray<VariableResultValue>.Empty;
        }

        private ImmutableArray<VariableResultValue> ResolveVariableDeclarator(VariableDeclaratorSyntax variableDeclarator) {
            if(variableDeclarator.Initializer != null) {
                return new[] {new VariableResultValue(variableDeclarator.Initializer.Value) }.ToImmutableArray();
            }

            return ImmutableArray<VariableResultValue>.Empty;
        }
        #endregion
    }
}
