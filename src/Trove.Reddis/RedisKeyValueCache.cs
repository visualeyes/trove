using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Redis {
    internal class RedisKeyValueCache<V> : ISourceBackedCache<V> where V : class {

        private readonly IDatabase db;
        private readonly string cacheName;
        private readonly Type valueType;

        public RedisKeyValueCache(IDatabase db, string cacheName) {
            this.db = db;
            this.cacheName = cacheName;
            this.valueType = typeof(V);
        }

        public async Task<V> GetAsync(string key, IKeyValueSource<V> provider) {
            string cacheKey = this.GetCacheKey(key);
            RedisValue redisVal = await db.StringGetAsync(cacheKey).ConfigureAwait(false);

            var item = ConvertValue(redisVal);

            if (item == null) {
                item = await provider.GetAsync(key);
                await this.StoreAsync(key, item);
            }

            return item;
        }

        public async Task LoadFromProviderAsync(IKeyValueSource<V> provider) {
            var allItems = await provider.GetAllAsync();

            var tasks = new List<Task>();

            foreach (var item in allItems) {
                tasks.Add(this.StoreAsync(item.Key, item.Value));
            }
            
            await Task.WhenAll(tasks);
        }

        private static bool Exists(RedisValue redisVal) {
            return redisVal.HasValue && !redisVal.IsNullOrEmpty;
        }

        private V ConvertValue(RedisValue redisVal) {
            if (!Exists(redisVal)) {
                return null;
            }

            if (valueType == typeof(string)) {
                return redisVal as V;
            } else {
                var value = JsonConvert.DeserializeObject<V>(redisVal);
                return value;
            }
        }

        private Task StoreAsync(string key, V value) {
            RedisValue redisVal;

            if (valueType == typeof(string)) {
                redisVal = value as string;
            } else {
                redisVal = JsonConvert.SerializeObject(value);
            }

            string cacheKey = this.GetCacheKey(key);
            return db.StringSetAsync(cacheKey, redisVal);
        }

        private string GetCacheKey(string key) {
            return String.Format("{0}-{1}", cacheName, key);
        }
    }
}
