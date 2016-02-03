using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Memcached {
    public static class TroveCacheFactoryExtensions {
        public static ITroveCacheFactory RegisterMemcachedCacheFactory(this ITroveCacheFactory factory) {
            Contract.NotNull(factory, nameof(factory));

            factory.RegisterFactory<MemcachedProviderConfig>((config) => new MemcachedCacheProviderFactory(config));
            return factory;
        }
    }
}
