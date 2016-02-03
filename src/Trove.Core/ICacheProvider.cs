using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trove.Core {
    public interface ICacheProvider<V> where V : class {
        bool SupportsFlushing { get; }

        Task<V> GetAsync(string key);
        Task SetAsync(string key, V value);
        Task SetAsync(IDictionary<string, V> keyValues, bool flush = false);
        Task FlushAsync();
    }
}
