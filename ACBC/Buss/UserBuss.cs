using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACBC.Common;
using ACBC.Dao;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ACBC.Buss
{
    public class UserBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.UserApi;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public object Do_UserLogin(BaseApi baseApi)
        {
            UserLoginParam userLoginParam = JsonConvert.DeserializeObject<UserLoginParam>(baseApi.param.ToString());
            if (userLoginParam==null)
            {
                 throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }
            if (userLoginParam.userName == null || userLoginParam.userName == "")
            {
                throw new ApiException(CodeMessage.InterfaceValueError, "InterfaceValueError");
            }
            if (userLoginParam.password == null || userLoginParam.password == "")
            {
                throw new ApiException(CodeMessage.InterfaceValueError, "InterfaceValueError");
            }
            string password = MD5Manager.MD5Encrypt32(userLoginParam.password);
            UserDao userDao = new UserDao();
            UserLoginItem userLoginItem = new UserLoginItem();
            userLoginItem = userDao.UserLogin(userLoginParam.userName, password);
            string token = Util.SaveUserMessage(userLoginItem);
            if (token != "")
            {
                userLoginItem.token = token;
                userLoginItem.isonload = true;
                return userLoginItem;
            }
            else
            {
                throw new ApiException(CodeMessage.ErrorLogin, "ErrorLogin");
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string Do_UserLogout(BaseApi baseApi)
        {           
            UserLoginParam userLoginParam = JsonConvert.DeserializeObject<UserLoginParam>(baseApi.param.ToString());
            if (!Util.DeleteRedis(baseApi.token))
            {
                throw new ApiException(CodeMessage.ErrorLogout, "ErrorLogout");
            }           
            return "";
        }
    }    
}
