using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Redis {
    public static class TroveCacheFactoryExtensions {
        public static ITroveCacheFactory RegisterRedisCacheFactory(this ITroveCacheFactory factory) {
            Contract.NotNull(factory, nameof(factory));

            factory.RegisterFactory<RedisProviderConfig>((config) => new RedisCacheProviderFactory(config));
            return factory;
        }
    }
}
