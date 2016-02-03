using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trove.Redis;
using Xunit;

namespace Trove.Tests.Redis {
    public class RedisProviderConfigFacts {

        [Fact]
        public void Null_Multiplexer_Throws() {
            IConnectionMultiplexer redis = null;
            Assert.Throws<ArgumentNullException>(() => new RedisProviderConfig(redis));
        }
    }
}
