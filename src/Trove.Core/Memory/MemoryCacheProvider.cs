using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trove.Core.Memory {
    internal class MemoryCacheProvider<V> : ICacheProvider<V> where V : class {
        private readonly ConcurrentDictionary<string, V> cache = new ConcurrentDictionary<string, V>();

        public bool SupportsFlushing {
            get { return true; }
        }

        public Task<V> GetAsync(string key) {
            Contract.NotNullOrEmpty(key, nameof(key));

            V value;
            cache.TryGetValue(key, out value);

            return Task.FromResult(value);
        }
        
        public Task SetAsync(string key, V value) {
            Contract.NotNullOrEmpty(key, nameof(key));

            SetInternal(key, value);

            return Task.FromResult(0);
        }

        public Task SetAsync(IDictionary<string, V> keyValues, bool flush = false) {
            Contract.NotNull(keyValues, nameof(keyValues));

            if (flush) {
                FlushInternal();
            }

            foreach(var item in keyValues) {
                SetInternal(item.Key, item.Value);
            }

            return Task.FromResult(0);
        }

        public Task FlushAsync() {
            FlushInternal();
            return Task.FromResult(0);
        }
        
        private void SetInternal(string key, V value) {
            Contract.NotNullOrEmpty(key, nameof(key));

            cache.AddOrUpdate(key, value, (k, existing) => value);
        }

        private void FlushInternal() {
            cache.Clear();
        }
    }
}
