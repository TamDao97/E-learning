using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.Common.RedisCache
{
    public class RedisCacheSettings : IRedisCacheSettings
    {
        public string PrefixSystemKey { get; set; }
        public string PrefixLoginKey { get; set; }
        public string ConnectionString { get; set; }
    }

    public class IRedisCacheSettings
    {
    }
}
