using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;
using Xunit;

namespace Trove.Tests.Core {
    [Collection("TroveCacheFactory")]
    public class TroveCacheFactoryFacts {
        private readonly TroveCacheFactory factory;

        public TroveCacheFactoryFacts() {
            this.factory = new TroveCacheFactory();
        }

        [Fact]
        public void Register_Factory_Null_Factory_Throws() {
            Func<ICacheProviderConfig, ICacheProviderFactory> providerFactory = null;
            Assert.Throws<ArgumentNullException>(() => factory.RegisterFactory(providerFactory));
        }

        [Fact]
        public void Get_Cache_Factory_Null_Config_Throws() {
            ICacheProviderConfig config = null;
            Assert.Throws<ArgumentNullException>(() => factory.GetCacheProviderFactory(config));
        }

        [Fact]
        public void Register_And_Get_Factory() {
            var mockConfig = new Mock<ICacheProviderConfig>();
            var mockProviderFactory = new Mock<ICacheProviderFactory>();

            // Can't get config before it's registered
            Assert.Throws<ApplicationException>(() => factory.GetCacheProviderFactory(mockConfig.Object));

            factory.RegisterFactory<ICacheProviderConfig>((config) => mockProviderFactory.Object);

            var providerFactory = factory.GetCacheProviderFactory(mockConfig.Object);
            Assert.Equal(mockProviderFactory.Object, providerFactory);
        }
        
    }
}
