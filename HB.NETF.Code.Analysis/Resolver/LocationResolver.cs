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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace HB.NETF.Code.Analysis.Resolver {
    public static class LocationResolver {
        public static async Task<IEnumerable<Location>> FindCallerLocations(this ISymbol symbol, Solution solution, IImmutableSet<Document> documents, CancellationToken token = default) {
            IEnumerable<SymbolCallerInfo> callerInfo = await SymbolFinder.FindCallersAsync(symbol, solution, documents);
            return callerInfo.SelectMany(e => e.Locations);
        }

        public static async Task<IEnumerable<Location>> FindReferenceLocations(this ISymbol symbol, Solution solution, IImmutableSet<Document> documents, CancellationToken token = default) {
            IEnumerable<ReferencedSymbol> references = await SymbolFinder.FindReferencesAsync(symbol, solution, documents, token);
            return references.SelectMany(l => l.Locations.Select(s => s.Location));
        }

        public static IEnumerable<Location> GetLocationsDistinct(IEnumerable<Location> locations) {
            return locations.GroupBy(e => e.SourceTree.FilePath).Select(e => e.First());
        }

        public static IEnumerable<Location> GetLocationsWithDifferentSemanticsThanSource(this IEnumerable<Location> locations, string sourcePath) {
            return locations.Where(e => e.SourceTree.FilePath != sourcePath);
        }

        public static IEnumerable<Location> GetLocationsWithSameSemanticsAsSource(this IEnumerable<Location> locations, string sourcePath) {
            return locations.Where(e => e.SourceTree.FilePath == sourcePath);
        }

        /// <summary>
        /// Make sure the <paramref name="factory"/> has a valid <see cref="SemanticModelCache"/> that contains the <see cref="SemanticModel"/> of given <paramref name="locations"/>.
        /// </summary>
        /// <typeparam name="TAnalyser"></typeparam>
        /// <param name="locations"></param>
        /// <param name="solution"></param>
        /// <param name="project"></param>
        /// <param name="semanticModelCache"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<TAnalyser, IEnumerable<SyntaxNode>>> GetAnalyserWithNodesToAnalyse<TAnalyser>(IEnumerable<Location> locations, Solution solution, Project project, IAnalyserFactory factory) where TAnalyser : ICodeAnalyser {
            foreach(IGrouping<string, Location> group in locations.GroupBy(e => e.SourceTree.FilePath)) {
                if(factory.SemanticModelCache.TryGet(group.Key, out SemanticModel semanticModel)) {
                    TAnalyser analyser = factory.GetOrCreateAnalyser<TAnalyser>(solution, project, semanticModel);
                    yield return new Tuple<TAnalyser, IEnumerable<SyntaxNode>>(analyser, group.Select(e => GetNodeFromLocation(e)));
                }
            }
        }

        public static SyntaxNode GetNodeFromLocation(Location location) => location.SourceTree.GetRoot().FindNode(location.SourceSpan);

        public static IEnumerable<SyntaxNode> GetNodesFromLocations(this IEnumerable<Location> locations) {
            foreach(Location location in locations) {
                yield return GetNodeFromLocation(location);
            }
        }

    }
}
