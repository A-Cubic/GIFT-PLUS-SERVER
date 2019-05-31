using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Common
{
    public class Util
    {
        public static List<UserMessage> GetUserMessage(string token)
        {
            List<UserMessage> list = new List<UserMessage>();
            UserMessage userMessage = new UserMessage();
            using (var client = ConnectionMultiplexer.Connect(Global.Redis))
            {
                var db = client.GetDatabase(0);
                string[] redisValue = db.StringGet(token).ToString().Split(",");
                userMessage.userId = redisValue[0];
                userMessage.shopId = redisValue[1];
                userMessage.power = redisValue[2];
            }          
            list.Add(userMessage);           
            return list;
        }
    }
    public class UserMessage
    {
        public string userId;
        public string shopId;
        public string power;
    }

}
