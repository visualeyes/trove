using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trove.Core.Memory {
    internal class MemoryKeyValueCache<V> : ISourceBackedCache<V> where V : class {
        private readonly static ConcurrentDictionary<string, V> cache = new ConcurrentDictionary<string, V>();

        public async Task<V> GetAsync(string key, IKeyValueSource<V> provider) {
            V item;
                
            if (!cache.TryGetValue(key, out item)) {
                item = await provider.GetAsync(key);
                SetItem(key, item);
            }
                
            return item;
        }
        
        public async Task LoadFromProviderAsync(IKeyValueSource<V> provider) {
            var items = await provider.GetAllAsync().ConfigureAwait(false);
            
            cache.Clear();

            foreach(var item in items) {
                SetItem(item.Key, item.Value);
            }
        }

        private static void SetItem(string key, V item) {
            cache.AddOrUpdate(key, item, (k, existing) => item);
        }
    }
}
