using Com.ACBC.Framework.Database;
using ACBC.Buss;
using ACBC.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ACBC.Common
{
    public class Global
    {

        public const string ENV = "PRO";

        public const string GROUP = "Gift-Admin";

        public const string CONFIG_TOPIC = "ConfigServerTopic";

        public const string TOPIC_MESSAGE = "update";

        public const string NAMESPACE = "com.a-cubic.gift";
        public const string ROUTE_PX = "/api/giftmanage";
        public const int REDIS_DB = 11;
        public static BaseBuss BUSS = new BaseBuss();
        public static Dictionary<string, Dictionary<string, ConfigGroup>> ConfigList;

        static Action<ChannelMessage> action = new Action<ChannelMessage>(onMessageHandle);

        public static void StartUp()
        {
            GetConfig(true);
            if (DatabaseOperationWeb.TYPE == null)
            {
                DatabaseOperationWeb.TYPE = new DBManager();
            }           
            RedisManager.ConfigurationOption = Redis;
        }

        public static void onMessageHandle(ChannelMessage channelMessage)
        {
            if (channelMessage.Message.ToString() == TOPIC_MESSAGE)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "收到配置更新通知");
                GetConfig(false);
            }
        }

        static void Subscribe()
        {
            var redis = RedisManager.getRedisConn();
            var queue = redis.GetSubscriber().Subscribe(CONFIG_TOPIC + "." + ENV + "." + GROUP);

            queue.OnMessage(action);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "已订阅" + CONFIG_TOPIC + "." + ENV + "." + GROUP + "配置更新");
        }

        static void GetConfig(bool isFirst)
        {
            string url = "http://ConfigServer/api/config/Config/Open";
            GetConfigParam configParam = new GetConfigParam
            {
                env = ENV,
                group = GROUP
            };
            RequestParam requestParam = new RequestParam
            {
                method = "GetConfig",
                param = configParam
            };
            string body = JsonConvert.SerializeObject(requestParam);
            try
            {
                string resp = Util.HttpPost(url,body, "application / json");
                ResponseObj responseObj = JsonConvert.DeserializeObject<ResponseObj>(resp);
                foreach (ConfigItem item in responseObj.data)
                {
                    Environment.SetEnvironmentVariable(item.key, item.value);
                }

                DatabaseOperation.TYPE = new DBManager();
                RedisManager.ConfigurationOption = Redis;
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "加载配置信息完成");
                if (isFirst)
                {
                    Subscribe();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(url);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "加载配置信息失败");
            }
        }

        public static string Redis
        {
            get
            {
                return Environment.GetEnvironmentVariable("redis");
            }
        }

        public static string DBUrl
        {
            get
            {
                return Environment.GetEnvironmentVariable("MysqlDBUrl");
            }
        }

        public static string DBUser
        {
            get
            {
                return Environment.GetEnvironmentVariable("MysqlDBUser");
            }
        }

        public static string DBPort
        {
            get
            {
                return Environment.GetEnvironmentVariable("MysqlDBPort");
            }
        }

        public static string DBPassword
        {
            get
            {
                return Environment.GetEnvironmentVariable("MysqlDBPassword");
            }
        }

        public static string AppId
        {
            get
            {
                return Environment.GetEnvironmentVariable("AppId");
            }
        }

        public static string Secret
        {
            get
            {
                return Environment.GetEnvironmentVariable("Secret");
            }
        }
    }
}
