using StackExchange.Redis;

namespace Trove.Redis {
    public interface IRedisProviderConfig {
        IConnectionMultiplexer Redis { get; }
    }
}