using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Memcached {
    public class MemcachedProviderConfig : ICacheProviderConfig {
        public MemcachedProviderConfig(IMemcachedClient client) {
            this.MemcachedClient = client;
        }

        public IMemcachedClient MemcachedClient { get; private set; }
    }
}
