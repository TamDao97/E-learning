using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.Common.RedisCache
{
    public static class RedisCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCache = configuration.GetSection("RedisCacheSettings");
            services.Configure<RedisCacheSettings>(redisCache);

            services.AddSingleton<IRedisCacheConnectionFactory, RedisCacheConnectionFactory>();
            services.AddSingleton<RedisCacheService>();

            return services;
        }
    }
}
