using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Models {
    public class SemanticModelCache {
        private readonly Dictionary<string, SemanticModel> modelCache = new Dictionary<string, SemanticModel>();
        public SemanticModelCache() { }

        public SemanticModelCache(IImmutableSet<Document> documents) {
            foreach(Document document in documents) {
                if(document.FilePath != null && document.TryGetSemanticModel(out SemanticModel semanticModel)) {
                    modelCache.Add(document.FilePath, semanticModel);
                }
            }
        }

        public bool TryGet(string filePath, out SemanticModel semanticModel) {
            semanticModel = Get(filePath);
            return semanticModel != null;
        }

        public SemanticModel Get(string filePath) {
            if (modelCache.TryGetValue(filePath, out var model)) return model;

            return null;
        }

        public void Add(string filePath, SemanticModel model) {
            modelCache[filePath] = model;
        }
    }
}
