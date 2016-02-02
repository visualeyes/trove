using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trove.Core {
    public interface ICacheProviderFactory {
        ISourceBackedCache<V> GetSourceBackedCache<V>(string name) where V : class;
    }
}
