using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Memcached {
    internal class MemcachedCacheProviderFactory : ICacheProviderFactory {
        private readonly MemcachedProviderConfig config;
        
        public MemcachedCacheProviderFactory(MemcachedProviderConfig config) {
            this.config = config;
        }
        
        public ISourceBackedCache<V> GetSourceBackedCache<V>(string name) where V : class {
            return new MemcachedSourceBackedCache<V>(config.MemcachedClient, name);
        }
    }
}
