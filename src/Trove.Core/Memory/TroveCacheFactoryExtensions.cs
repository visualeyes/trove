using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Core.Memory {
    public static class TroveCacheFactoryExtensions {
        public static ITroveCacheFactory RegisterMemoryCacheFactory(this ITroveCacheFactory factory) {
            factory.RegisterFactory<MemoryProviderConfig>((config) => new MemoryCacheProviderFactory());
            return factory;
        }
    }
}
