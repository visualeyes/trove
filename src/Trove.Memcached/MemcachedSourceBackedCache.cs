using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Memcached {
    internal class MemcachedSourceBackedCache<V> : ISourceBackedCache<V> where V : class {
        private readonly IMemcachedClient client;
        private readonly string cacheName;

        public MemcachedSourceBackedCache(IMemcachedClient client, string cacheName) {
            this.client = client;
            this.cacheName = cacheName;
        }

        public async Task<V> GetAsync(string key, IKeyValueSource<V> provider) {
            var item = client.Get<V>(GetMemcacheKey(key));

            if (item == null) {
                item = await provider.GetAsync(key);
                this.Store(key, item);
            }

            return item;
        }

        public async Task LoadFromProviderAsync(IKeyValueSource<V> provider) {
            var allItems = await provider.GetAllAsync();

            foreach (var item in allItems) {
                this.Store(item.Key, item.Value);
            }
        }

        private void Store(string key, V item) {
            client.Store(StoreMode.Set, GetMemcacheKey(key), item);
        }

        private string GetMemcacheKey(string key) {
            return String.Format("{0}-{1}", cacheName, key);
        }
    }
}
