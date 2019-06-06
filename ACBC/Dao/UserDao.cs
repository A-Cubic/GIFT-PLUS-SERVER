using ACBC.Buss;
using Com.ACBC.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Dao
{
    public class UserDao
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public UserLoginItem UserLogin(string userName, string password)
        {
            UserLoginItem userLoginItem = new UserLoginItem();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(UserDaoSqls.SELECT_T_BASE_WEB_USER_BY_USERCODE_AND_PASSWORD, userName, password );
            string select = stringBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select,"T").Tables[0];
            if (dt.Rows.Count==1 && dt.Rows[0][0]!=DBNull.Value)
            {
                userLoginItem.userCode= dt.Rows[0]["userCode"].ToString();
                userLoginItem.name = dt.Rows[0]["USERNAME"].ToString();
                userLoginItem.shopId= dt.Rows[0]["store_id"].ToString();
                userLoginItem.power= dt.Rows[0]["userType"].ToString();
                userLoginItem.authority = dt.Rows[0]["userType"].ToString() == "1" ? "admin" : "employee";
                return userLoginItem;
            }
            else
            {
                return userLoginItem;
            }
        }
    }

    public class UserDaoSqls
    {
        public const string SELECT_T_BASE_WEB_USER_BY_USERCODE_AND_PASSWORD = ""
            + " SELECT USERCODE,STORE_ID,USERTYPE,USERNAME "
            + " FROM T_BASE_WEB_USER "
            + " WHERE USERCODE='{0}' "
            + " AND PASSWORD='{1}'";
    }
}
