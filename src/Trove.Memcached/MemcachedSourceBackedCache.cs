using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Memcached {
    internal class MemcachedSourceBackedCache<V> : ICacheProvider<V> where V : class {
        private readonly IMemcachedClient client;
        private readonly string cacheName;

        public MemcachedSourceBackedCache(IMemcachedClient client, string cacheName) {
            this.client = client;
            this.cacheName = cacheName;
        }

        public bool SupportsFlushing {
            get { return false; }
        }

        public Task<V> GetAsync(string key) {
            string cachekey = GetMemcacheKey(key);
            var item = client.Get<V>(cachekey);
            return Task.FromResult(item);
        }

        public Task SetAsync(string key, V value) {
            this.SetItem(key, value);
            return Task.FromResult(0);
        }

        public Task SetAsync(IDictionary<string, V> keyValues, bool flush = false) {
            if(flush) {
                throw new NotImplementedException();
            }
            
            foreach (var item in keyValues) {
                this.SetItem(item.Key, item.Value);
            }

            return Task.FromResult(0);
        }

        public Task FlushAsync() {
            throw new NotImplementedException();
        }
        
        private void SetItem(string key, V item) {
            client.Store(StoreMode.Set, GetMemcacheKey(key), item);
        }

        private string GetMemcacheKey(string key) {
            return String.Format("{0}-{1}", cacheName, key);
        }

    }
}
