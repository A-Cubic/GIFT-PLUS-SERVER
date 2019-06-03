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
    public class GoodsBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.GoodsApi;
        }

        /// <summary>
        /// 库存管理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>     
        public PageResult Do_StockStatistics(BaseApi baseApi)
        {
            PageResult pageResult = new PageResult();
            pageResult.list = new List<object>();
            pageResult.item = new object();
            StockStatisticsParam stockStatisticsParam = JsonConvert.DeserializeObject<StockStatisticsParam>(baseApi.param.ToString());
            if (stockStatisticsParam.current == 0)
            {
                stockStatisticsParam.current = 1;               
            }
            if (stockStatisticsParam.pageSize==0)
            {
                stockStatisticsParam.pageSize = 10;
            }
            if (stockStatisticsParam.status == null || stockStatisticsParam.status == "" || stockStatisticsParam.status == "预到店")
            {
                stockStatisticsParam.status = " and v.state='1' ";
            }
            else if(stockStatisticsParam.status == "已到店")
            {
                stockStatisticsParam.status = " and v.state!='1' ";
            }
            pageResult.pagination = new Page(stockStatisticsParam.current, stockStatisticsParam.pageSize);
            StockStatisticsItem stockStatisticsItem = new StockStatisticsItem();
            var userId = "";
            var shopId = "";
            using (var client= ConnectionMultiplexer.Connect(Global.Redis))
            {
                var db = client.GetDatabase(0);
                string[] redisValue = db.StringGet(baseApi.token).ToString().Split(",");
                userId = redisValue[0];
                shopId= redisValue[1];
            }
            GoodsDao giftManageDao = new GoodsDao();
            int allGoods = 0;
            int movinGoods = 0;
            int arriveGoods = 0;
            DataTable dt=giftManageDao.StockStatistics(shopId);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    allGoods += Convert.ToInt16(dt.Rows[i]["num"]);
                    if (dt.Rows[i]["state"].ToString() == "1")
                    {
                        movinGoods = Convert.ToInt16(dt.Rows[i]["num"]);
                    }
                    else
                    {
                        arriveGoods += Convert.ToInt16(dt.Rows[i]["num"]);
                    }
                }
            }
            stockStatisticsItem.allGoods = allGoods.ToString()+"个商品";
            stockStatisticsItem.movinGoods = movinGoods.ToString() + "个商品";
            stockStatisticsItem.arriveGoods = arriveGoods.ToString() + "个商品";
            pageResult.item = stockStatisticsItem;
            dt = giftManageDao.StockStatisticsList(shopId, stockStatisticsParam);
            if (dt.Rows.Count>0)
            {
                for (int i= (stockStatisticsParam.current - 1) * stockStatisticsParam.pageSize; i< dt.Rows.Count && i< stockStatisticsParam.current * stockStatisticsParam.pageSize; i++)
                {
                    StockStatisticsList stockStatisticsList = new StockStatisticsList();
                    stockStatisticsList.key =  i + 1;
                    stockStatisticsList.goodsName = dt.Rows[i]["goods_name"].ToString();
                    stockStatisticsList.goodsnum = dt.Rows[i]["num"].ToString();
                    stockStatisticsList.goodsImg = dt.Rows[i]["goods_img"].ToString();
                    stockStatisticsList.proportion = (Math.Round( Convert.ToDouble(dt.Rows[i]["num"]) / allGoods * 100, 2)).ToString();
                    pageResult.list.Add(stockStatisticsList);
                }
            }
            pageResult.pagination.total = dt.Rows.Count;
            return pageResult;
        }
    }
    
}
