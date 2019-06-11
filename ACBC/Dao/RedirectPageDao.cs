using Com.ACBC.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Dao
{
    public class RedirectPageDao
    {
        public string RedirectPage(string code)
        {
            string page = "";
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(RedirectPageSqls.SELEECT_PAGE_URL_FROM_T_BASE_REDIRECT_PAGE, code);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            if (dt.Rows.Count>0)
            {
                page = dt.Rows[0]["PAGE_URL"].ToString();
            }
            return page;
        }
    }

    public class RedirectPageSqls
    {
        public static string SELEECT_PAGE_URL_FROM_T_BASE_REDIRECT_PAGE = ""
            + " SELECT PAGE_URL "
            + " FROM T_BASE_REDIRECT_PAGE "
            + " WHERE PAGE_CODE='{0}'";

    }
}
