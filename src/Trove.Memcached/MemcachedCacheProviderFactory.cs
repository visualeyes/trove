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
        private readonly IMemcachedProviderConfig config;
        
        public MemcachedCacheProviderFactory(IMemcachedProviderConfig config) {
            Contract.NotNull(config, nameof(config));
            this.config = config;
        }
        
        public ICacheProvider<V> GetCacheProvider<V>(string name) where V : class {
            Contract.NotNullOrEmpty(name, nameof(name));

            return new MemcachedSourceBackedCache<V>(config.MemcachedClient, name);
        }
    }
}
