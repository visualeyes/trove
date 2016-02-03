using Enyim.Caching;
using Trove.Core;

namespace Trove.Memcached {
    public interface IMemcachedProviderConfig {
        IMemcachedClient MemcachedClient { get; }
    }
}