using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;
using Trove.Core.Memory;
using Xunit;

namespace Trove.Tests.Core {
    public class SourceBackedCacheFacts {
        private readonly Mock<ICacheProvider<object>> mockCacheProvider;
        private readonly Mock<IKeyValueSource<object>> mockSource;
        private readonly SourceBackedCache<object> cache;

        public SourceBackedCacheFacts() {
            this.mockCacheProvider = new Mock<ICacheProvider<object>>();
            this.mockSource = new Mock<IKeyValueSource<object>>();
            this.cache = new SourceBackedCache<object>(this.mockCacheProvider.Object);
        }

        [Fact]
        public async Task Get_Missing_Key_From_Provider() {
            string key = nameof(Get_Missing_Key_From_Provider);
            object item = new object();

            this.mockCacheProvider.Setup(p => p.GetAsync(key)).ReturnsAsync(null);
            this.mockSource.Setup(p => p.GetAsync(key)).ReturnsAsync(item);

            var cachedItem = await this.cache.GetAsync(key, this.mockSource.Object);

            this.mockCacheProvider.Verify(p => p.GetAsync(key), Times.Once);
            this.mockSource.Verify(p => p.GetAsync(key), Times.Once);

            Assert.Equal(item, cachedItem);
        }

        [Fact]
        public async Task Get_Existing_Key_From_Cache() {
            string key = nameof(Get_Existing_Key_From_Cache);
            object item = new object();

            this.mockCacheProvider.Setup(p => p.GetAsync(key)).ReturnsAsync(item);

            var cachedItem = await this.cache.GetAsync(key, this.mockSource.Object);

            this.mockCacheProvider.Verify(p => p.GetAsync(key), Times.Once);
            this.mockSource.Verify(p => p.GetAsync(key), Times.Never);

            Assert.Equal(item, cachedItem);
        }

        [Fact]
        public async Task Handle_Missing_Throw() {
            string key = nameof(Handle_Missing_Throw);

            this.mockCacheProvider.Setup(p => p.GetAsync(key)).ReturnsAsync(null);
            this.mockSource.Setup(p => p.GetAsync(key)).ReturnsAsync(null);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await this.cache.GetAsync(key, this.mockSource.Object, ProviderDefaultValueHandling.Throw));
        }

        [Fact]
        public async Task Handle_Missing_NoStore() {
            string key = nameof(Handle_Missing_NoStore);

            this.mockCacheProvider.Setup(p => p.GetAsync(key)).ReturnsAsync(null);
            this.mockSource.Setup(p => p.GetAsync(key)).ReturnsAsync(null);

            var cachedItem = await this.cache.GetAsync(key, this.mockSource.Object, ProviderDefaultValueHandling.NoStore);

            this.mockCacheProvider.Verify(p => p.GetAsync(key), Times.Once);
            this.mockCacheProvider.Verify(p => p.SetAsync(key, cachedItem), Times.Never);
            this.mockSource.Verify(p => p.GetAsync(key), Times.Once);

            Assert.Null(cachedItem);
        }

        [Fact]
        public async Task Handle_Missing_Store() {
            string key = nameof(Handle_Missing_Store);

            this.mockCacheProvider.Setup(p => p.GetAsync(key)).ReturnsAsync(null);
            this.mockSource.Setup(p => p.GetAsync(key)).ReturnsAsync(null);

            var cachedItem = await this.cache.GetAsync(key, this.mockSource.Object, ProviderDefaultValueHandling.Store);

            this.mockCacheProvider.Verify(p => p.GetAsync(key), Times.Once);
            this.mockCacheProvider.Verify(p => p.SetAsync(key, cachedItem), Times.Once);
            this.mockSource.Verify(p => p.GetAsync(key), Times.Once);

            Assert.Null(cachedItem);
        }

        [Theory]
        [InlineData(true, true), InlineData(false, true), InlineData(false, false)]
        public async Task Load_From_Provider(bool flush, bool supportsFlushing) {
            var items = new Dictionary<string, object>();

            this.mockCacheProvider.SetupGet(p => p.SupportsFlushing).Returns(supportsFlushing);

            this.mockSource.Setup(s => s.GetAllAsync()).ReturnsAsync(items);
            this.mockCacheProvider.Setup(p => p.SetAsync(items, flush)).Returns(Task.FromResult(0));

            await this.cache.LoadFromProviderAsync(this.mockSource.Object, flush: flush);

            this.mockSource.Verify(s => s.GetAllAsync(), Times.Once);
            this.mockCacheProvider.Verify(p => p.SetAsync(items, flush), Times.Once);
        }

        [Fact]
        public async Task Load_From_Provider_Unsupported_Flushing_Throws() {
            this.mockCacheProvider.SetupGet(p => p.SupportsFlushing).Returns(false);
            
            await Assert.ThrowsAsync<NotSupportedException>(async () => await this.cache.LoadFromProviderAsync(this.mockSource.Object, flush: true));
        }
    }
}
