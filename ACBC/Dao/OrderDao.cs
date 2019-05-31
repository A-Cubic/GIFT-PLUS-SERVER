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
            selectBuilder.AppendFormat("select state,order_code,sum(num) num ,sum(goods_price) price,pay_time " +
                "from v_order_info " +
                "where store_id='{0}' {1} {2} {3} group by order_code order by pay_time desc ", shopId, order, date, state);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select,"T").Tables[0];
            return dt;
        }
    }
}
