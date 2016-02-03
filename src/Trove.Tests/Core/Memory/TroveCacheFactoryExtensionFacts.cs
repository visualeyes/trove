using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;
using Trove.Core.Memory;
using Xunit;

namespace Trove.Tests.Core.Memory {
    public class TroveCacheFactoryExtensionFacts {
    
        [Fact]
        public void Register_Memory_Cache_Null_Throws() {
            ITroveCacheFactory factory = null;
            Assert.Throws<ArgumentNullException>(() => factory.RegisterMemoryCacheFactory());
        }

        [Fact]
        public void Register_Memory_Cache() {
            var mockCacheFactory = new Mock<ITroveCacheFactory>();

            mockCacheFactory.Object.RegisterMemoryCacheFactory();

            mockCacheFactory.Verify(f => f.RegisterFactory(It.IsAny<Func<MemoryProviderConfig, ICacheProviderFactory>>()), Times.Once);
        }
    }
}
