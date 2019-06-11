using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACBC.Buss;
using ACBC.Common;
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

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageResult OrderDetails(OrderDetailsParam orderDetailsParam)
        {
            PageResult pageResult = new PageResult();
            pageResult.list = new List<object>();
            pageResult.item = orderDetailsParam.orderCode;
            pageResult.pagination = new Page(orderDetailsParam.current, orderDetailsParam.pageSize);
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(OrderDaoSqls.SELECT_ORDER_GOODS_FROM_V_ORDER_INFO, orderDetailsParam.orderCode);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            if (dt.Rows.Count>0)
            {
                for (int i = (orderDetailsParam.current - 1) * orderDetailsParam.pageSize; i < dt.Rows.Count && i < orderDetailsParam.current * orderDetailsParam.pageSize; i++)
                {
                    OrderDetailsItem orderDetailsItem = new OrderDetailsItem();
                    orderDetailsItem.key = i + 1;
                    orderDetailsItem.goodsId = dt.Rows[i]["GOODS_ID"].ToString();
                    orderDetailsItem.goodsName = dt.Rows[i]["GOODS_NAME"].ToString();
                    orderDetailsItem.barcode = dt.Rows[i]["BARCODE"].ToString();
                    orderDetailsItem.num = dt.Rows[i]["NUM"].ToString();
                    orderDetailsItem.supplyPrice = dt.Rows[i]["GOODS_COST"].ToString();
                    orderDetailsItem.salePrice = dt.Rows[i]["GOODS_PRICE"].ToString();
                    if (dt.Rows[i]["GOODS_STATE"].ToString() == "0")
                    {
                        orderDetailsItem.state = "未支付";
                    }
                    else if (dt.Rows[i]["GOODS_STATE"].ToString() == "1")
                    {
                        orderDetailsItem.state = "已支付";
                    }
                    else if (dt.Rows[i]["GOODS_STATE"].ToString() == "2")
                    {
                        orderDetailsItem.state = "已到店";
                    }
                    else if (dt.Rows[i]["GOODS_STATE"].ToString() == "3")
                    {
                        orderDetailsItem.state = "已取走";
                    }
                    pageResult.list.Add(orderDetailsItem);
                }
            }
            pageResult.pagination.total = dt.Rows.Count;
            return pageResult;
        }
    }

    public class OrderDaoSqls
    {
        public const string SELECT_V_ORDER_INFO_BY_STORE_ID = ""
            + " SELECT V.STATE,V.ORDER_CODE,SUM(V.NUM) NUM ,SUM(V.GOODS_PRICE) PRICE,V.PAY_TIME,M.MEMBER_IMG "
            + " FROM V_ORDER_INFO V,T_BASE_MEMBER M "
            + " WHERE V.MEMBER_NAME=M.MEMBER_NAME AND STORE_ID='{0}' {1} {2} {3} "
            + " GROUP BY ORDER_CODE "
            + " ORDER BY PAY_TIME DESC";

        public const string SELECT_ORDER_GOODS_FROM_V_ORDER_INFO = ""
            + " SELECT V.NUM,V.GOODS_NAME,V.GOODS_ID,V.BARCODE,V.GOODS_PRICE,V.GOODS_COST,G.GOODS_STATE "
            + " FROM V_ORDER_INFO V,T_BUSS_ORDER_GOODS G"
            + " WHERE V.ORDER_CODE=G.ORDER_CODE  AND V.GOODS_ID=G.GOODS_ID  AND  V.ORDER_CODE='{0}' "
            + " ORDER BY GOODS_ID ASC";
    }
}
