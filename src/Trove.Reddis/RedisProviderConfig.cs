using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Redis {
    public class RedisProviderConfig : ICacheProviderConfig, IRedisProviderConfig {
        public RedisProviderConfig(IConnectionMultiplexer redis) {
            Contract.NotNull(redis, nameof(redis));

            this.Redis = redis;
        }

        public IConnectionMultiplexer Redis { get; private set; }
    }
}
