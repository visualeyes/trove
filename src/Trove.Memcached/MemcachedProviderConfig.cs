using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Memcached {
    public class MemcachedProviderConfig : ICacheProviderConfig, IMemcachedProviderConfig {
        public MemcachedProviderConfig(IMemcachedClient client) {
            Contract.NotNull(client, nameof(client));

            this.MemcachedClient = client;
        }

        public IMemcachedClient MemcachedClient { get; private set; }
    }
}
