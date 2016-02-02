using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Core {
    public class TroveCacheFactory : ITroveCacheFactory {
        private static ConcurrentDictionary<Type, Func<ICacheProviderConfig, ICacheProviderFactory>> keyValueFactoryCache = new ConcurrentDictionary<Type, Func<ICacheProviderConfig, ICacheProviderFactory>>();

        public ICacheProviderFactory GetCacheProviderFactory<T>(T config) where T : ICacheProviderConfig {
            if (config == null) throw new ArgumentNullException(nameof(config));

            var configType = typeof(T); // this is what it was registered with - config.GetType() would get the implementation

            Func<ICacheProviderConfig, ICacheProviderFactory> factory;

            if (!keyValueFactoryCache.TryGetValue(configType, out factory)) {
                throw new ApplicationException("No provider is registered for a config of type: " + configType.FullName);
            }

            var provider = factory(config);

            return provider;
        }

        public void RegisterFactory<T>(Func<T, ICacheProviderFactory> providerFactory) where T : ICacheProviderConfig {
            if (providerFactory == null) throw new ArgumentNullException(nameof(providerFactory));

            var interfaceFactory = providerFactory as Func<ICacheProviderConfig, ICacheProviderFactory>;

            keyValueFactoryCache.AddOrUpdate(typeof(T), interfaceFactory, (key, existing) => interfaceFactory);
        }
    }
}
