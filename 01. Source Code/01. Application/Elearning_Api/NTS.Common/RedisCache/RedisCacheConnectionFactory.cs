using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.Common.RedisCache
{
    public class RedisCacheConnectionFactory : IRedisCacheConnectionFactory
    {
        private readonly Lazy<ConnectionMultiplexer> _connection;

        public RedisCacheConnectionFactory(IOptions<RedisCacheSettings> options)
        {
            this._connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options.Value.ConnectionString));
        }

        public ConnectionMultiplexer Connection()
        {
            return this._connection.Value;
        }
    }

    public interface IRedisCacheConnectionFactory
    {
        ConnectionMultiplexer Connection();
    }
}
