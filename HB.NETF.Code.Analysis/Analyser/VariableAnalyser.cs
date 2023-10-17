using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Code.Analysis.Models;
using HB.NETF.Code.Analysis.Resolver;
using HB.NETF.Common.DependencyInjection;
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
        private readonly IAnalyserFactory analyserFactory;
        private readonly IIdentifierAnalyser identifierAnalyser;
        internal VariableAnalyser(Solution solution, Project project, SemanticModel semanticModel) : base(solution, project, semanticModel) {
            analyserFactory = DIContainer.GetService<IAnalyserFactory>();
            identifierAnalyser = analyserFactory.GetOrCreateAnalyser<IdentifierAnalyser>(solution, project, semanticModel);
        }

        public async Task<VariableResult?> Run(SyntaxNode syntaxNode) {
            ImmutableArray<VariableResultValue> resultValues = await ResolveInternal(syntaxNode);
            this.resultValues.Clear();
            return new VariableResult(syntaxNode, resultValues);
        }

        async Task<object> ICodeAnalyser.Run(SyntaxNode syntaxNode) => await Run(syntaxNode);

        public async Task<IdentifierNameSyntax[]> GetPossibleIdentifiersFromSnapshot(SyntaxNode node) {
            ImmutableArray<IdentifierResult> result = await identifierAnalyser.Run(node);
            return result.Where(e => e.Kind.HasValue
                    && (e.Kind.Value == SymbolKind.Local
                    || e.Kind.Value == SymbolKind.Parameter
                    || e.Kind.Value == SymbolKind.Field
                    || e.Kind.Value == SymbolKind.Property))
                .Select(e => e.Value).ToArray();
        }


        #region Resolve
        private readonly List<VariableResultValue> resultValues = new List<VariableResultValue>();
        private async Task<ImmutableArray<VariableResultValue>> ResolveInternal(SyntaxNode syntaxNode) {
            switch(syntaxNode) {
                case IdentifierNameSyntax identifier:
                    await ResolveIdentifier(identifier); 
                    break;
            }

            return resultValues.ToImmutableArray();
        }

        private async Task ResolveIdentifier(IdentifierNameSyntax identifier) {
            ISymbol identifierSymbol = SemanticModel.GetSymbolInfo(identifier).Symbol;
            if (identifierSymbol == null)
                return;

            SyntaxReference declarationReference = identifierSymbol.DeclaringSyntaxReferences.FirstOrDefault();
            if (declarationReference == null)
                return;

            SyntaxNode declaration = await declarationReference.GetSyntaxAsync();

            IEnumerable<Location> locations = await LocationResolver.FindReferenceLocations(identifierSymbol, Solution, Documents);
            IEnumerable<Tuple<VariableAnalyser, IEnumerable<SyntaxNode>>> analyserTuples = LocationResolver.GetAnalyserWithNodesToAnalyse<VariableAnalyser>(locations, Solution, Project, analyserFactory);

            foreach (Tuple<VariableAnalyser, IEnumerable<SyntaxNode>> analyserTuple in analyserTuples) {
                VariableAnalyser analyser = analyserTuple.Item1;
                IEnumerable<SyntaxNode> nodes = analyserTuple.Item2;

                List<Task<ImmutableArray<VariableResultValue>>> analyserTasks = new List<Task<ImmutableArray<VariableResultValue>>>();
                foreach (SyntaxNode node in nodes)
                    analyserTasks.Add(analyser.ResolveInternal(node));

                resultValues.AddRange((await Task.WhenAll(analyserTasks)).SelectMany(e => e));
            }

            switch (declaration) {
                case ParameterSyntax parameter:
                    await ResolveParameter(parameter);
                    break;
                case PropertyDeclarationSyntax propertyDeclaration:
                    await ResolvePropertyDeclaration(propertyDeclaration);
                    break;
                case VariableDeclaratorSyntax variableDeclarator:
                    await ResolveVariableDeclarator(variableDeclarator);
                    break;
            }
        }

        private async Task ResolveParameter(ParameterSyntax parameter) {
            ISymbol declaredSymbol = SemanticModel.GetDeclaredSymbol(parameter.Parent.Parent);
            if (declaredSymbol is null)
                return;

            IEnumerable<Location> locations = await LocationResolver.FindCallerLocations(declaredSymbol, Solution, Documents);
            IEnumerable<Tuple<VariableAnalyser, IEnumerable<SyntaxNode>>> analyserTuples = LocationResolver.GetAnalyserWithNodesToAnalyse<VariableAnalyser>(locations, Solution, Project, analyserFactory);

            List<VariableResultValue> recAnalyserResult = new List<VariableResultValue>();

            foreach(Tuple<VariableAnalyser, IEnumerable<SyntaxNode>> analyserTuple in analyserTuples) {
                VariableAnalyser analyser = analyserTuple.Item1;
                IEnumerable<SyntaxNode> nodes = analyserTuple.Item2;

                List<Task<ImmutableArray<VariableResultValue>>> analyserTasks = new List<Task<ImmutableArray<VariableResultValue>>>();
                foreach(SyntaxNode node in nodes)
                    analyserTasks.Add(analyser.ResolveInternal(node));

                recAnalyserResult.AddRange((await Task.WhenAll(analyserTasks)).SelectMany(e => e));
            }

            resultValues.AddRange(recAnalyserResult);
        }

        private async Task ResolvePropertyDeclaration(PropertyDeclarationSyntax propertyDeclaration) {
            if(propertyDeclaration.Initializer == null) 
                return;

            resultValues.Add(new VariableResultValue(propertyDeclaration.Initializer.Value));

            resultValues.AddRange(await this.ResolveInternal(propertyDeclaration.Initializer.Value));
        }

        private async Task ResolveVariableDeclarator(VariableDeclaratorSyntax variableDeclarator) {
            if (variableDeclarator.Initializer == null)
                return;

            resultValues.Add(new VariableResultValue(variableDeclarator.Initializer.Value));

            resultValues.AddRange(await this.ResolveInternal(variableDeclarator.Initializer.Value));
        }
        #endregion
    }
}
