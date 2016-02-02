using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;
using Trove.Redis;

namespace Trove.Redis {
    internal class RedisCacheProviderFactory : ICacheProviderFactory {
        private readonly RedisProviderConfig config;

        public RedisCacheProviderFactory(RedisProviderConfig config) {
            this.config = config;
        }

        public ISourceBackedCache<V> GetSourceBackedCache<V>(string cacheName) where V : class {
            var db = config.Redis.GetDatabase();
            return new RedisKeyValueCache<V>(db, cacheName);
        }
    }
}
