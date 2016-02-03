using Enyim.Caching;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Memcached;
using Xunit;

namespace Trove.Tests.Memcached {
    public class MemcachedCacheProviderFactoryFacts {

        [Fact]
        public void Get_Cache_Provider_Null_Config_Throws() {
            IMemcachedProviderConfig config = null;
            Assert.Throws<ArgumentNullException>(() => new MemcachedCacheProviderFactory(config));
        }

        [Theory]
        [InlineData(null), InlineData(""), InlineData(" ")]
        public void Get_Cache_Provider_Empty_Name_Throws(string name) {
            var mockConfig = new Mock<IMemcachedProviderConfig>();
            var factory = new MemcachedCacheProviderFactory(mockConfig.Object);

            Assert.Throws<ArgumentNullException>(() => factory.GetCacheProvider<object>(name));
        }


        [Fact]
        public void Get_Cache_Provider() {
            var mockConfig = new Mock<IMemcachedProviderConfig>();
            var mockClient = new Mock<IMemcachedClient>();
            mockConfig.SetupGet(c => c.MemcachedClient).Returns(mockClient.Object);

            var factory = new MemcachedCacheProviderFactory(mockConfig.Object);

            var cacheProvider = factory.GetCacheProvider<object>("cache");
            Assert.NotNull(cacheProvider);
        }
    }
}
