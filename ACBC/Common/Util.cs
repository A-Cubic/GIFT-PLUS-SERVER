﻿using ACBC.Buss;
using ACBC.Dao;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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

        public static bool SaveRedis(string key,string value,int time)
        {
            try
            {
                using (var client = ConnectionMultiplexer.Connect(Global.Redis))
                {
                    TimeSpan timeSpan = new TimeSpan(0, 0, time);
                    var db = client.GetDatabase(0);
                    db.StringSet(key, value, timeSpan);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public static string GetRedis(string token)
        {
            string redisValue = "";
            using (var client = ConnectionMultiplexer.Connect(Global.Redis))
            {
                var db = client.GetDatabase(0);
                redisValue = db.StringGet(token).ToString();                
            }
            return redisValue;
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

        public static string HttpPost(string url,string body,string contentType)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = contentType;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 20000;
            byte[] tbyte = Encoding.UTF8.GetBytes(body);
            httpWebRequest.ContentLength = tbyte.Length;
            httpWebRequest.GetRequestStream().Write(tbyte, 0, tbyte.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();
            httpWebRequest.Abort();
            httpWebResponse.Close();

            return responseContent;
        }

        public static string HttpGet(string url,string contentType)
        {           
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = contentType;
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();
            httpWebRequest.Abort();
            httpWebResponse.Close();

            return responseContent;
        }

        public static string Sha1(string str)
        {
             byte[] buffer = Encoding.UTF8.GetBytes(str);
             byte[] data = SHA1.Create().ComputeHash(buffer);

             StringBuilder sb = new StringBuilder();
             foreach (byte t in data)
             {
                 sb.Append(t.ToString("X2"));
             }
             
             return sb.ToString();
        }
    }
   

}
