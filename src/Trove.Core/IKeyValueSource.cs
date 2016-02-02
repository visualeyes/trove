using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trove.Core {
    public interface IKeyValueSource<V> {
        Task<V> GetAsync(string key);
        Task<IDictionary<string, V>> GetAllAsync();
    }
}
