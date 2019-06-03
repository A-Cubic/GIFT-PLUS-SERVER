using ACBC.Buss;
using ACBC.Dao;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Common
{
    public class Util
    {
        public static string GetUserUserId(string token)
        {
            string userId = "";
            using (var client = ConnectionMultiplexer.Connect(Global.Redis))
            {
                var db = client.GetDatabase(0);
                string[] redisValue = db.StringGet(token).ToString().Split(",");
                userId = redisValue[0];
            }                               
            return userId;
        }

        public static string GetUserShopId(string token)
        {
            string shopId = "";
            using (var client = ConnectionMultiplexer.Connect(Global.Redis))
            {
                var db = client.GetDatabase(0);
                string[] redisValue = db.StringGet(token).ToString().Split(",");               
                shopId = redisValue[1];                
            }
            return shopId;
        }

        public static string  GetUserPower(string token)
        {
            string power = "";
            using (var client = ConnectionMultiplexer.Connect(Global.Redis))
            {
                var db = client.GetDatabase(0);
                string[] redisValue = db.StringGet(token).ToString().Split(",");
                power = redisValue[2];
            }
            return power;
        }

        public  static string SaveUserMessage(UserLoginItem userLoginItem)
        {
            string token = "";
            if (userLoginItem.userCode != "" && userLoginItem.userCode != null)
            {
                token = MD5Manager.createToken(userLoginItem.userCode);
                using (var client = ConnectionMultiplexer.Connect(Global.Redis))
                {
                    TimeSpan timeSpan = new TimeSpan(0, 30, 0);
                    var db = client.GetDatabase(0);
                    db.StringSet(token, userLoginItem.userCode + "," + userLoginItem.shopId + "," + userLoginItem.power, timeSpan);                   
                    return token;
                }
            }
            else
            {
                return token;
            }
        }

        public static bool DeleteRedis(string token)
        {
            using (var client = ConnectionMultiplexer.Connect(Global.Redis))
            {
                var db = client.GetDatabase(0);
                if (db.KeyDelete(token))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
   

}
