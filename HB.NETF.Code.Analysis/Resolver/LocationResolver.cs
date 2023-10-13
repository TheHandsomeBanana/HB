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
        public static async Task<IEnumerable<Location>> FindCallerLocations(ISymbol symbol, Solution solution, IImmutableSet<Document> documents, CancellationToken token = default) {
            IEnumerable<SymbolCallerInfo> callerInfo = await SymbolFinder.FindCallersAsync(symbol, solution, documents);
            return callerInfo.SelectMany(e => e.Locations);
        }

        public static async Task<IEnumerable<Location>> FindReferenceLocations(ISymbol symbol, Solution solution, IImmutableSet<Document> documents, CancellationToken token = default) {
            IEnumerable<ReferencedSymbol> references = await SymbolFinder.FindReferencesAsync(symbol, solution, documents, token);
            return references.SelectMany(l => l.Locations.Select(s => s.Location));
        }

        public static IEnumerable<Location> GetLocationsDistinct(IEnumerable<Location> locations) {
            return locations.GroupBy(e => e.SourceTree.FilePath).Select(e => e.First());
        }
    }
}
