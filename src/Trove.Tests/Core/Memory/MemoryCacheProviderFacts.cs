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
    public class MemoryCacheProviderFacts {
        private const string ValidKey = "key";
        private readonly MemoryCacheProvider<object> cache;

        public MemoryCacheProviderFacts() {
            this.cache = new MemoryCacheProvider<object>();
        }
        
        [Fact]
        public void Supports_Flushing() {
            Assert.True(this.cache.SupportsFlushing);
        }

        [Theory]
        [InlineData(null), InlineData(""), InlineData(" ")]
        public async Task Get_Empty_Key_Throws(string key) {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.cache.GetAsync(key));
        }

        [Fact]
        public async Task Get_Returns_Default_For_Missing_Key() {
            var item = await this.cache.GetAsync(ValidKey);
            Assert.Null(item);
        }

        [Theory]
        [InlineData(null), InlineData(""), InlineData(" ")]
        public async Task Set_Empty_Key_Throws(string key) {
            object item = new object();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.cache.SetAsync(key, item));
        }

        [Fact]
        public async Task Set_Key_Item() {
            string key = ValidKey;
            object item = new object();

            await this.cache.SetAsync(key, item);

            var cachedItem = await this.cache.GetAsync(key);

            Assert.Equal(item, cachedItem);
        }

        [Fact]
        public async Task Set_Null_value() {
            string key = ValidKey;
            object item = null;

            await this.cache.SetAsync(key, item);

            var cachedItem = await this.cache.GetAsync(key);

            Assert.Equal(item, cachedItem);
        }

        [Fact]
        public async Task Set_Null_Dict_Throws() {
            Dictionary<string, object> keyValues = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.cache.SetAsync(keyValues));
        }

        [Fact]
        public async Task Set_Dict() {
            var item = new object();

            var keyValues = new Dictionary<string, object>() {
                { ValidKey, item }
            };

            await this.cache.SetAsync(keyValues);

            var cachedItem = await this.cache.GetAsync(ValidKey);
            Assert.Equal(item, cachedItem);
        }

        [Theory]
        [InlineData(""), InlineData(" ")]
        public async Task Set_Dict_Empty_Key(string key) {
            var keyValues = new Dictionary<string, object>() {
                { key, new object() }
            };

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.cache.SetAsync(keyValues));
        }

        [Fact]
        public async Task Set_Dict_Flush() {
            string staleKey = ValidKey + "-stable";
            var staleItem = new object();
            
            await this.cache.SetAsync(staleKey, staleItem);
            var cachedStaleItem = await this.cache.GetAsync(staleKey);

            Assert.Equal(staleItem, cachedStaleItem);

            var item = new object();

            var keyValues = new Dictionary<string, object>() {
                { ValidKey, item }
            };

            await this.cache.SetAsync(keyValues, flush: true);

            cachedStaleItem = await this.cache.GetAsync(staleKey);
            Assert.Null(cachedStaleItem);

            var cachedItem = await this.cache.GetAsync(ValidKey);
            Assert.Equal(item, cachedItem);
        }

        [Fact]
        public async Task Flush() {
            var keyValues = new Dictionary<string, object>() {
                { ValidKey, new object() }
            };

            await this.cache.SetAsync(keyValues, flush: false);

            await this.cache.FlushAsync();

            var staleItem = await this.cache.GetAsync(ValidKey);
            
            Assert.Null(staleItem);
        }

        [Fact]
        public async Task Flush_Empty() {
            await this.cache.FlushAsync();
        }

    }
}
