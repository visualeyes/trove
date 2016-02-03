using Moq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Redis;
using Xunit;

namespace Trove.Tests.Redis {
    public class RedisCacheProviderFactoryFacts {
        [Fact]
        public void Get_Cache_Provider_Null_Config_Throws() {
            IRedisProviderConfig config = null;
            Assert.Throws<ArgumentNullException>(() => new RedisCacheProviderFactory(config));
        }

        [Theory]
        [InlineData(null), InlineData(""), InlineData(" ")]
        public void Get_Cache_Provider_Empty_Name_Throws(string name) {
            var mockConfig = new Mock<IRedisProviderConfig>();
            var factory = new RedisCacheProviderFactory(mockConfig.Object);

            Assert.Throws<ArgumentNullException>(() => factory.GetCacheProvider<object>(name));
        }


        [Fact]
        public void Get_Cache_Provider() {
            var mockConfig = new Mock<IRedisProviderConfig>();
            var mockMultiPlexer = new Mock<IConnectionMultiplexer>();
            mockConfig.SetupGet(c => c.Redis).Returns(mockMultiPlexer.Object);

            var factory = new RedisCacheProviderFactory(mockConfig.Object);

            var cacheProvider = factory.GetCacheProvider<object>("cache");
            Assert.NotNull(cacheProvider);
        }
    }
}
