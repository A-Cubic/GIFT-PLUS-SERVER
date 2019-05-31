using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACBC.Buss;
using Com.ACBC.Framework.Database;

namespace ACBC.Dao
{
    public class MemberDao
    {
        public DataTable MemberList(string shopId, MemberListParam memberListParam)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select s.reg_phone,m.member_name,m.member_img,m.member_sex " +
                " from t_buss_member_store s ,t_base_member m  where s.member_id=m.member_id  and s.store_id='{0}' {1} {2} ", shopId, memberListParam.sex, memberListParam.userName);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }
    }
}
