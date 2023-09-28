using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Cache {
    public class SemanticModelCache {
        private readonly Dictionary<string, SemanticModel> modelCache = new Dictionary<string, SemanticModel>();
        public SemanticModelCache() { }

        public SemanticModel Get(string filePath) {
            if(modelCache.TryGetValue(filePath, out var model)) return model;

            return null;
        }

        public void Add(string filePath,  SemanticModel model) {
            modelCache[filePath] = model;
        }
    }
}
