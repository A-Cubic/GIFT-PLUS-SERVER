using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ACBC.Common;
using ACBC.Dao;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ACBC.Buss
{
    public class OrderBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.OrderApi;
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageResult Do_OrderList(BaseApi baseApi)
        {
            PageResult pageResult = new PageResult();
            pageResult.list = new List<object>();
            OrderListItemList orderListItemList = new OrderListItemList();          
            OrderListParam orderListParam = JsonConvert.DeserializeObject<OrderListParam>(baseApi.param.ToString());
            var order = "";
            var date = "";
            var state = "";
            if (orderListParam.orderCode != null && orderListParam.orderCode != "")
            {
                order = " and order_code  like '%"+ orderListParam.orderCode + "%'" ;
            }
            if (orderListParam.date!=null && orderListParam.date.Length==2  )
            {
                date = " and  (pay_time between '"+ orderListParam.date[0] + "' and '"+ orderListParam.date[1] + "')";
            }
            if ( orderListParam.state == "预到店")
            {
                state = " and state='1' ";
            }
            else if(orderListParam.state == "已到店")
            {
                state = " and state!='1' ";
            }
            if (orderListParam.current == 0)
            {
                orderListParam.current = 1;
            }
            if (orderListParam.pageSize == 0)
            {
                orderListParam.pageSize = 10;
            }
            pageResult.pagination = new Page(orderListParam.current, orderListParam.pageSize);
            var userId = Util.GetUserUserId(baseApi.token);
            var shopId = Util.GetUserShopId(baseApi.token);
            
            OrderDao orderDao = new OrderDao();
            DataTable dt= orderDao.OrderList(shopId, order, date,  state);
            if (dt.Rows.Count>0)
            {
                for (int i= (orderListParam.current - 1) * orderListParam.pageSize; i< dt.Rows.Count && i<orderListParam.current * orderListParam.pageSize; i++)
                {                    
                    OrderListItem orderListItem = new OrderListItem();
                    orderListItem.key=  i + 1;
                    orderListItem.img= dt.Rows[i]["MEMBER_IMG"].ToString();
                    orderListItem.num = dt.Rows[i]["num"].ToString();
                    orderListItem.orderCode= dt.Rows[i]["order_code"].ToString();
                    orderListItem.state = dt.Rows[i]["state"].ToString()=="1"?"预到店":"已到店";
                    orderListItem.payTime= dt.Rows[i]["pay_time"].ToString();
                    orderListItem.price = dt.Rows[i]["price"].ToString();
                    orderListItemList.totalPrice += Convert.ToDouble(orderListItem.price);
                    pageResult.list.Add(orderListItem);
                }                
            }
            pageResult.pagination.total = dt.Rows.Count;
            pageResult.item = orderListItemList;
            return pageResult;
        }
    }
   
}
