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
            selectBuilder.AppendFormat(MemberDaoSqls.SELECT_T_BUSS_MEMBER_STORE_AND_T_BASE_MEMBER_BY_STORE_ID, shopId, memberListParam.sex, memberListParam.userName);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }
    }

    public class MemberDaoSqls
    {
        public const string SELECT_T_BUSS_MEMBER_STORE_AND_T_BASE_MEMBER_BY_STORE_ID = ""
            + " SELECT S.REG_PHONE,M.MEMBER_NAME,M.MEMBER_IMG,M.MEMBER_SEX "
            + "  FROM T_BUSS_MEMBER_STORE S ,T_BASE_MEMBER M  "
            + " WHERE S.MEMBER_ID=M.MEMBER_ID "
            + " AND S.STORE_ID='{0}' {1} {2}";
    }
}
