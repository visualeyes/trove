using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;
using Trove.Redis;

namespace Trove.Redis {
    internal class RedisCacheProviderFactory : ICacheProviderFactory {
        private readonly IRedisProviderConfig config;

        public RedisCacheProviderFactory(IRedisProviderConfig config) {
            Contract.NotNull(config, nameof(config));

            this.config = config;
        }

        public ICacheProvider<V> GetCacheProvider<V>(string cacheName) where V : class {
            Contract.NotNullOrEmpty(cacheName, nameof(cacheName));

            var db = config.Redis.GetDatabase();
            return new RedisCacheProvider<V>(db, cacheName);
        }
    }
}
