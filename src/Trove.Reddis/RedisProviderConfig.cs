using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Core;

namespace Trove.Redis {
    public class RedisProviderConfig : ICacheProviderConfig {
        public RedisProviderConfig(ConnectionMultiplexer redis) {
            this.Redis = redis;
        }

        public ConnectionMultiplexer Redis { get; private set; }
    }
}
