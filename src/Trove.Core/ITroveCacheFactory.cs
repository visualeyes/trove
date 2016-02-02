using System;
using Trove.Core;

namespace Trove.Core {
    public interface ITroveCacheFactory {
        ICacheProviderFactory GetCacheProviderFactory<T>(T config) where T : ICacheProviderConfig;
        void RegisterFactory<T>(Func<T, ICacheProviderFactory> providerFactory) where T : ICacheProviderConfig;
    }
}
