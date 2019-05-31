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
            
            if (userLoginItem.userCode != "" && userLoginItem.userCode != null)
            {
                string token = MD5Manager.createToken(userLoginItem.userCode);
                using (var client = ConnectionMultiplexer.Connect(Global.Redis))
                {
                    TimeSpan timeSpan = new TimeSpan(0,30,0);
                    var db = client.GetDatabase(0);
                    db.StringSet(token,  userLoginItem.userCode + ","+ userLoginItem.shopId + "," + userLoginItem.power, timeSpan);
                    userLoginItem.token = token;
                    userLoginItem.isonload = true;                    
                    return userLoginItem;
                }
            }
            else
            {
                userLoginItem.isonload = false;
                userLoginItem.msg = "用户名或密码错误";
            }

            return userLoginItem;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public MsgResult Do_UserLogout(BaseApi baseApi)
        {
            MsgResult msg = new MsgResult();
            UserLoginParam userLoginParam = JsonConvert.DeserializeObject<UserLoginParam>(baseApi.param.ToString());
            using (var client = ConnectionMultiplexer.Connect(Global.Redis))
            {
                var db = client.GetDatabase(0);
                if (db.KeyDelete(baseApi.token))
                {
                    msg.type = 1;
                }
                else
                {
                    msg.msg = "登出失败";
                }                 
            }
            return msg;
        }
    }

    public class UserLoginParam
    {
        public string userName;//用户名
        public string password;//密码
    }
    public class UserLoginItem
    {
        public string userCode;//用户Code
        public string token;//token
        public string power;//权限
        public string authority;//前台权限
        public string shopId;//店铺信息
        public bool isonload;//是否登陆成功
        public string msg;//登录失败原因
    }
}
