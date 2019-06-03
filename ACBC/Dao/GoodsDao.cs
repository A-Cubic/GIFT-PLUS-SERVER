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
            selectBuilder.AppendFormat(GoodsDaoSqls.SELECT_V_ORDER_INFO_BY_STORE_ID, shopId);
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
            selectBuilder.AppendFormat(GoodsDaoSqls.SELECT_V_ORDER_INFO_AND_T_BUSS_GOODS, shopId, stockStatisticsParam.status);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];

            return dt;
        }
    }

    public class GoodsDaoSqls
    {
        public const string SELECT_V_ORDER_INFO_AND_T_BUSS_GOODS = ""
            + " SELECT V.GOODS_NAME,SUM(V.NUM) NUM,G.GOODS_IMG  "
            + " FROM V_ORDER_INFO V,T_BUSS_GOODS G "
            + " WHERE V.BARCODE=G.BARCODE "
            + " AND STORE_ID='{0}' {1} "
            + " GROUP BY V.BARCODE "
            + " ORDER BY NUM DESC";

        public const string SELECT_V_ORDER_INFO_BY_STORE_ID = ""
            + " SELECT COUNT(BARCODE) BARCODE,SUM(NUM) NUM,STATE "
            + " FROM V_ORDER_INFO "
            + " WHERE STORE_ID='{0}' "
            + " GROUP BY STATE";
    }
}
