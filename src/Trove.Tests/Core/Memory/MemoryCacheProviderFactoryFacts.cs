using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core.Memory;
using Xunit;

namespace Trove.Tests.Core.Memory {
    public class MemoryCacheProviderFactoryFacts {
        private readonly MemoryCacheProviderFactory factory;

        public MemoryCacheProviderFactoryFacts() {
            this.factory = new MemoryCacheProviderFactory();
        }

        [Theory]
        [InlineData(null), InlineData(""), InlineData(" ")]
        public void Get_Cache_Provider_Empty_Cache_Name_Throws(string name) {
            Assert.Throws<ArgumentNullException>(() => factory.GetCacheProvider<object>(name));
        }

        [Fact]
        public void Get_Cache_Provider() {
            string cache1Name = "cache-1";
            string cache2Name = "cache-2";

            var cache1 = this.factory.GetCacheProvider<object>(cache1Name);
            
            Assert.NotNull(cache1);

            var cache1Again = this.factory.GetCacheProvider<object>(cache1Name);
            Assert.Equal(cache1, cache1Again);

            var cache2 = this.factory.GetCacheProvider<object>(cache2Name);

            Assert.NotNull(cache2);
            Assert.NotEqual(cache1, cache2);
        }

        [Fact]
        public void Get_Source_Backed_Cache_Invalid_Type() {
            string cache3Name = "cache-3";

            var cache3 = this.factory.GetCacheProvider<MemoryCacheProviderFactoryFacts>(cache3Name);

            Assert.Throws<ApplicationException>(() => this.factory.GetCacheProvider<object>(cache3Name));
        }
    }
}
