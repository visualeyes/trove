using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trove.Core.Memory {
    internal class MemoryCacheStore : ICacheProviderFactory {
        private static ConcurrentDictionary<string, IMemoryKeyValueCache> store = new ConcurrentDictionary<string, IMemoryKeyValueCache>(StringComparer.OrdinalIgnoreCase);       
        
        public ISourceBackedCache<V> GetSourceBackedCache<V>(string name) where V : class {
            var cache = store.GetOrAdd(name, (key) => new MemoryKeyValueCache<V>());

            if (!(cache is MemoryKeyValueCache<V>)) {
                throw new ApplicationException(
                    String.Format(
                        "Unexpected cache type. Cache {0} is of type {1} when {2} was expected",
                        name, cache?.GetType().FullName, typeof(MemoryKeyValueCache<V>).FullName
                    )
                );
            }

            return cache as MemoryKeyValueCache<V>;
        }
    }
}
