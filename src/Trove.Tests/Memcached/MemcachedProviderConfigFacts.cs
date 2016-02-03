using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Memcached;
using Xunit;

namespace Trove.Tests.Memcached {
    public class MemcachedProviderConfigFacts {

        [Fact]
        public void Null_Client_Throws() {
            IMemcachedClient client = null;
            Assert.Throws<ArgumentNullException>(() => new MemcachedProviderConfig(client));
        }
    }
}
