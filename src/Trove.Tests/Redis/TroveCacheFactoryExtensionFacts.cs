using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;
using Trove.Redis;
using Xunit;

namespace Trove.Tests.Redis {
    public class TroveCacheFactoryExtensionFacts {
        [Fact]
        public void Register_Memory_Cache_Null_Throws() {
            ITroveCacheFactory factory = null;
            Assert.Throws<ArgumentNullException>(() => factory.RegisterRedisCacheFactory());
        }

        [Fact]
        public void Register_Memory_Cache() {
            var mockCacheFactory = new Mock<ITroveCacheFactory>();

            mockCacheFactory.Object.RegisterRedisCacheFactory();

            mockCacheFactory.Verify(f => f.RegisterFactory(It.IsAny<Func<RedisProviderConfig, ICacheProviderFactory>>()), Times.Once);
        }
    }
}
