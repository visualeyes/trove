using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trove.Core.Memory {
    internal class MemoryCacheProviderFactory : ICacheProviderFactory {
        private static ConcurrentDictionary<string, object> store = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);       
        
        public ICacheProvider<V> GetCacheProvider<V>(string name) where V : class {
            Contract.NotNullOrEmpty(name, nameof(name));

            var cache = store.GetOrAdd(name, (key) => new MemoryCacheProvider<V>());

            if (!(cache is MemoryCacheProvider<V>)) {
                throw new ApplicationException(
                    String.Format(
                        "Unexpected cache type. Cache {0} is of type {1} when {2} was expected",
                        name, cache?.GetType().FullName, typeof(MemoryCacheProvider<V>).FullName
                    )
                );
            }

            return cache as MemoryCacheProvider<V>;
        }
    }
}
