using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Code.Analysis.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Analyser {
    public class TypeAnalyser : AnalyserBase, ITypeAnalyser {
        private const int MAXREC = 4;
        public Type[] TypeFilter { get; } = Array.Empty<Type>();

        internal TypeAnalyser(Solution solution, Project project, SemanticModel semanticModel) : base(solution, project, semanticModel) {
        }

        public TypeAnalyser(Solution solution, Project project, SemanticModel semanticModel, params Type[] filter) : this(solution, project, semanticModel) {
            this.TypeFilter = filter;
        }

        public async Task<TypeResult?> Run(SyntaxNode syntaxNode) => await GetFirstFromSnapshot(syntaxNode);
        async Task<object> ICodeAnalyser.Run(SyntaxNode syntaxNode) => await Run(syntaxNode);
        public async Task<TypeResult?> GetFirstFromSnapshot(SyntaxNode syntaxNode) {
            int i = 0;
            while (!(syntaxNode is ExpressionSyntax) && i < MAXREC) {
                syntaxNode = syntaxNode.Parent;
                i++;
            }

            if (syntaxNode is ExpressionSyntax expression)
                return await ResolveExpression(expression);

            return null;
        }

        public async Task<TypeResult[]> GetAll(SyntaxTree syntaxTree) {
            List<TypeResult> foundTypes = new List<TypeResult>();
            SyntaxNode root = await syntaxTree.GetRootAsync();
            foreach (SyntaxNode desc in root.DescendantNodesAndSelf()) {
                ITypeSymbol type = SemanticModel.GetTypeInfo(desc).Type;
                if (type is null)
                    continue;

                if (TypeFilter.Length > 0 && !TypeFilter.Any(e => e.FullName == type.ToDisplayString()))
                    continue;

                foundTypes.Add(new TypeResult(desc, type));
            }

            return foundTypes.ToArray();
        }

        private async Task<TypeResult?> ResolveExpression(SyntaxNode expression) {
            switch (expression) {
                case InvocationExpressionSyntax invocation:
                    return CheckType(invocation);
                case BinaryExpressionSyntax binary when binary.Kind() == SyntaxKind.EqualsExpression || binary.Kind() == SyntaxKind.NotEqualsExpression:
                    await ResolveExpression(binary.Left);
                    break;
                case MemberAccessExpressionSyntax memberAccess:
                    await ResolveExpression(memberAccess.Expression);
                    break;
                case ConditionalAccessExpressionSyntax conditionalAccess:
                    await ResolveExpression(conditionalAccess.Expression);
                    break;
                case IdentifierNameSyntax identifier:
                    return CheckType(identifier);
                case SwitchStatementSyntax switchStatement:
                    await ResolveExpression(switchStatement.Expression);
                    break;
                case AttributeArgumentListSyntax attributeArgument:
                    await ResolveExpression(attributeArgument.Parent);
                    break;
                case AttributeSyntax attribute:
                    await ResolveExpression(attribute.Name);
                    break;
            }

            return null;
        }

        private TypeResult? CheckType(ExpressionSyntax expression) {
            ITypeSymbol type = SemanticModel.GetTypeInfo(expression).Type;
            if (type == null)
                return null;

            if (TypeFilter.Length > 0 && !TypeFilter.Any(e => e.FullName == type.ToDisplayString()))
                return null;

            return new TypeResult(expression, type);
        }
    }

    public class TypeAnalyser<T> : ITypeAnalyser<T> {
        private readonly TypeAnalyser typeAnalyser;
        public Type TypeFilter { get; }

        public TypeAnalyser(Solution solution, Project project, SemanticModel semanticModel) {
            this.TypeFilter = typeof(T);

            typeAnalyser = new TypeAnalyser(solution, project, semanticModel, typeof(T));
        }

        public async Task<TypeResult?> Run(SyntaxNode syntaxNode) => await GetFirstFromSnapshot(syntaxNode);
        public async Task<TypeResult?> GetFirstFromSnapshot(SyntaxNode syntaxNode) => await typeAnalyser.GetFirstFromSnapshot(syntaxNode);
        public async Task<TypeResult[]> GetAll(SyntaxTree syntaxTree) => await typeAnalyser.GetAll(syntaxTree);

        async Task<object> ICodeAnalyser.Run(SyntaxNode syntaxNode) => await Run(syntaxNode);
    }
}
