﻿using HB.NETF.Code.Analysis.Interface;
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
    public class TypeAnalyser : ITypeAnalyser {
        private const int MAXREC = 4;

        public SemanticModel SemanticModel { get; }
        public Type[] TypeFilter { get; }

        public TypeAnalyser(SemanticModel semanticModel, params Type[] filter) {
            this.SemanticModel = semanticModel;
            this.TypeFilter = filter;
        }

        public async Task<SearchType?> GetFirst(SyntaxNode syntaxNode) {
            int i = 0;
            while (!(syntaxNode is ExpressionSyntax) && i < MAXREC) {
                syntaxNode = syntaxNode.Parent;
                i++;
            }

            if (syntaxNode is ExpressionSyntax expression)
                return await ResolveExpression(expression);

            return null;
        }

        public async Task<SearchType[]> GetAll(SyntaxNode syntaxNode) {
            throw new NotImplementedException();
        }

        private async Task<SearchType?> ResolveExpression(SyntaxNode expression) {
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

        private SearchType? CheckType(ExpressionSyntax expression) {
            ITypeSymbol type = SemanticModel.GetTypeInfo(expression).Type;
            if (type == null)
                return null;

            return new SearchType(expression, type);
        }
    }

    public class TypeAnalyser<T> : ITypeAnalyser<T> {
        private TypeAnalyser typeAnalyser;

        public SemanticModel SemanticModel { get; }
        public Type TypeFilter { get; }

        public TypeAnalyser(SemanticModel semanticModel) {
            this.TypeFilter = typeof(T);
            this.SemanticModel = semanticModel;

            typeAnalyser = new TypeAnalyser(semanticModel, typeof(T));
        }

        public Task<SearchType?> GetFirst(SyntaxNode syntaxNode) => typeAnalyser.GetFirst(syntaxNode);
        public Task<SearchType[]> GetAll(SyntaxNode syntaxNode) => typeAnalyser.GetAll(syntaxNode);
    }
}
