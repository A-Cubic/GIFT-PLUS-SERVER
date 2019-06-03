using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ACBC.Framework.Database;

namespace ACBC.Dao
{
    public class OrderDao
    {
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable OrderList(string shopId,string order,string date,string state)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(OrderDaoSqls.SELECT_V_ORDER_INFO_BY_STORE_ID, shopId, order, date, state);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select,"T").Tables[0];
            return dt;
        }
    }

    public class OrderDaoSqls
    {
        public const string SELECT_V_ORDER_INFO_BY_STORE_ID = ""
            + " SELECT STATE,ORDER_CODE,SUM(NUM) NUM ,SUM(GOODS_PRICE) PRICE,PAY_TIME "
            + " FROM V_ORDER_INFO "
            + " WHERE STORE_ID='{0}' {1} {2} {3} "
            + " GROUP BY ORDER_CODE "
            + " ORDER BY PAY_TIME DESC";
    }
}
