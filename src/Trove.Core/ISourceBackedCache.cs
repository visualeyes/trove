using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trove.Core {
    public interface ISourceBackedCache<V> where V : class {

        Task<V> GetAsync(string key, IKeyValueSource<V> provider);
        
        /// <summary>
        /// Loads entire cache from the source
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        Task LoadFromProviderAsync(IKeyValueSource<V> provider);
    }
}
