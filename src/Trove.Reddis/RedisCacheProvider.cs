using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Redis {
    internal class RedisCacheProvider<V> : ICacheProvider<V> where V : class {

        private readonly IDatabase db;
        private readonly string cacheName;
        private readonly bool valueTypeIsString;

        public RedisCacheProvider(IDatabase db, string cacheName) {
            this.db = db;
            this.cacheName = cacheName;
            this.valueTypeIsString = typeof(V) == typeof(string);
        }

        public bool SupportsFlushing {
            get { return true; }
        }

        public async Task<V> GetAsync(string key) {
            var value = await db.HashGetAsync(cacheName, key);

            var item = ConvertToValue(value);

            return item;
        }

        public async Task SetAsync(string key, V value) {
            RedisValue redisVal = ConvertFromValue(value);

            await db.HashSetAsync(cacheName, key, redisVal);
        }

        public async Task SetAsync(IDictionary<string, V> keyValues, bool flush = false) {
            if (flush) {
                await this.FlushAsync();
            }

            var entries = keyValues.Select(kv => {
                var value = ConvertFromValue(kv.Value);
                return new HashEntry(kv.Key, value);
            })
            .ToArray();

            await db.HashSetAsync(cacheName, entries);
        }
        
        public async Task FlushAsync() {
            await db.KeyDeleteAsync(cacheName);
        }

        private static bool Exists(RedisValue redisVal) {
            return redisVal.HasValue && !redisVal.IsNullOrEmpty;
        }

        private V ConvertToValue(RedisValue redisVal) {
            if (!Exists(redisVal)) {
                return null;
            }

            if (this.valueTypeIsString) {
                return redisVal as V;
            } else {
                var value = JsonConvert.DeserializeObject<V>(redisVal);
                return value;
            }
        }

        private RedisValue ConvertFromValue(V value) {
            RedisValue redisVal;
            if (this.valueTypeIsString) {
                redisVal = value as string;
            } else {
                redisVal = JsonConvert.SerializeObject(value);
            }

            return redisVal;
        }
    }
}
