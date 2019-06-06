using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Common
{
    public class RedisManager
    {
        private static ConfigurationOptions connDCS = ConfigurationOptions.Parse(Global.Redis);

        private static readonly object Locker = new object();

        private static ConnectionMultiplexer redisConn;

        public static object ConfigurationOption { get; internal set; }

        public static ConnectionMultiplexer getRedisConn()
        {
            if (redisConn == null)
            {
                lock (Locker)
                {
                    if (redisConn == null || !redisConn.IsConnected)
                    {
                        redisConn = ConnectionMultiplexer.Connect(connDCS);
                    }
                }
            }
            return redisConn;
        }

        public static RedisChannel ConfigServerTopic(string env, string group)
        {
            return new RedisChannel("ConfigServerTopic." + env + "." + group, RedisChannel.PatternMode.Literal);
        }
    }
}
