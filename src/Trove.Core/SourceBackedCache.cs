using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trove.Core {
    // Use cases
    // * Settings -> take note of flushing
    public class SourceBackedCache<V> : ISourceBackedCache<V> where V : class {
        private readonly ICacheProvider<V> cacheProvider;

        public SourceBackedCache(ICacheProvider<V> cacheProvider) {
            Contract.NotNull(cacheProvider, nameof(cacheProvider));

            this.cacheProvider = cacheProvider;
        }

        public async Task<V> GetAsync(string key, IKeyValueSource<V> provider, ProviderDefaultValueHandling defaultValueHandling = ProviderDefaultValueHandling.Throw) {
            Contract.NotNullOrEmpty(key, nameof(key));
            Contract.NotNull(provider, nameof(provider));

            var item = await this.cacheProvider.GetAsync(key);

            if (item == default(V)) {
                var providerItem = await provider.GetAsync(key);

                bool hasValue = providerItem != default(V);

                if (!hasValue && defaultValueHandling == ProviderDefaultValueHandling.Throw) {
                    throw new KeyNotFoundException(String.Format("Could not find value for '{0}' in the provider", key));
                }

                item = providerItem;

                if (hasValue || defaultValueHandling == ProviderDefaultValueHandling.Store) {
                    await this.cacheProvider.SetAsync(key, providerItem);
                }
            }

            return item;
        }

        public async Task LoadFromProviderAsync(IKeyValueSource<V> provider, bool flush = true) {
            Contract.NotNull(provider, nameof(provider));

            if (flush && !this.cacheProvider.SupportsFlushing) {
                throw new NotSupportedException("The provider does not support flushing the cache");
            }

            var items = await provider.GetAllAsync();

            await this.cacheProvider.SetAsync(items, flush: flush);
        }
    }
}
