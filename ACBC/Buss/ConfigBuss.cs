using ACBC.Common;
using ACBC.Dao;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Buss
{
    public class ConfigBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.OpenApi;
        }

        public object Do_GetConfig(BaseApi baseApi)
        {
            GetConfigParam getConfigParam = JsonConvert.DeserializeObject<GetConfigParam>(baseApi.param.ToString());
            if (getConfigParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }

            if (Global.ConfigList.ContainsKey(getConfigParam.env))
            {
                if (Global.ConfigList[getConfigParam.env].ContainsKey(getConfigParam.group))
                {
                    return (Global.ConfigList[getConfigParam.env])[getConfigParam.group].list;
                }
            }

            throw new ApiException(CodeMessage.InvalidEnvAndGroup, "InvalidEnvAndGroup");

        }

        public object Do_UpdateConfig(BaseApi baseApi)
        {
            UpdateConfigParam updateConfigParam = JsonConvert.DeserializeObject<UpdateConfigParam>(baseApi.param.ToString());
            if (updateConfigParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }

            ConfigDao configDao = new ConfigDao();
            var list = configDao.GetConfigList(updateConfigParam.env, updateConfigParam.group);
            if(list.Count > 0)
            {
                if (!Global.ConfigList.ContainsKey(updateConfigParam.env))
                {
                    Global.ConfigList.Add(updateConfigParam.env, new Dictionary<string, ConfigGroup>());
                }

                if (!Global.ConfigList[updateConfigParam.env].ContainsKey(updateConfigParam.group))
                {
                    Global.ConfigList[updateConfigParam.env].Add(updateConfigParam.group, new ConfigGroup(updateConfigParam.group));
                }

                Global.ConfigList[updateConfigParam.env][updateConfigParam.group].list = list;

                topic(updateConfigParam.env, updateConfigParam.group);
                return Global.ConfigList[updateConfigParam.env][updateConfigParam.group];
            }

            throw new ApiException(CodeMessage.InvalidEnvAndGroup, "InvalidEnvAndGroup");
        }

        public void GetConfigAll()
        {
            ConfigDao configDao = new ConfigDao();
            Global.ConfigList = configDao.GetConfigAll();
            foreach(string env in Global.ConfigList.Keys)
            {
                foreach (string group in Global.ConfigList[env].Keys)
                {
                    topic(env, group);
                }
            }
        }

        private void topic(string env, string group)
        {
            var redis = RedisManager.getRedisConn();
            var db = redis.GetDatabase(Global.REDIS_DB);
            db.Publish(RedisManager.ConfigServerTopic(env, group), "update");
        }
    }
}
