using ACBC.Buss;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Com.ACBC.Framework.Database;
using System.Threading.Tasks;

namespace ACBC.Dao
{
    public class GoodsDao
    {
        /// <summary>
        /// 商品统计
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>     
        public DataTable StockStatistics(string shopId)
        {            
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select count(barcode) barcode,sum(num) num,state from v_order_info where store_id='{0}' GROUP BY state", shopId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select,"T").Tables[0];

            return dt;
        }

        /// <summary>
        /// 商品统计
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>     
        public DataTable StockStatisticsList(string shopId, StockStatisticsParam stockStatisticsParam)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select v.goods_name,sum(v.num) num,g.goods_img " +
                                        "from v_order_info v,t_buss_goods g " +
                                        "WHERE v.barcode=g.barcode and store_id='{0}' {1}  GROUP BY v.barcode " +
                                        " order by num desc ", shopId, stockStatisticsParam.status);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];

            return dt;
        }
    }
}
