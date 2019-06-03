using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ACBC.Common;
using ACBC.Dao;
using Newtonsoft.Json;

namespace ACBC.Buss
{
    public class ActiveBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.ActiveApi;
        }

        /// <summary>
        /// 活动列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageResult Do_ActiveList(BaseApi baseApi)
        {
            ActiveListParam activeListParam = JsonConvert.DeserializeObject<ActiveListParam>(baseApi.param.ToString());
            if (activeListParam.current == 0)
            {
                activeListParam.current = 1;
            }
            if (activeListParam.pageSize == 0)
            {
                activeListParam.pageSize = 10;
            }
            if (activeListParam.title != "" && activeListParam.title != null)
            {
                activeListParam.title = " and a.remark like '%" + activeListParam.title + "%'";
            }
            else
            {
                activeListParam.title = "";
            }
            PageResult pageResult = new PageResult();
            pageResult.list = new List<object>();
            pageResult.pagination = new Page(activeListParam.current, activeListParam.pageSize);
            string shopId = Util.GetUserShopId(baseApi.token);
            ActiveDao activeDao = new ActiveDao();
            DataTable dt = activeDao.ActiveList(shopId, activeListParam);
            pageResult.pagination.total = 0;
            if (dt.Rows.Count>0)
            {
                DataView dataView = new DataView(dt);
                DataTable dt1 = dataView.ToTable(true, "active_id", "active_time_from", "active_time_to", "remark", "active_type");
                for (int i= (activeListParam.current - 1)* activeListParam.pageSize;i< dt1.Rows.Count && i< activeListParam.current * activeListParam.pageSize;i++)
                {
                    ActiveListItem activeListItem = new ActiveListItem();
                    activeListItem.img.Add("http://llwell-wxapp.oss-cn-beijing.aliyuncs.com/A-cubic/goods.png");
                    activeListItem.img.Add("http://llwell-wxapp.oss-cn-beijing.aliyuncs.com/A-cubic/heart.png");
                    activeListItem.img.Add("http://llwell-wxapp.oss-cn-beijing.aliyuncs.com/A-cubic/up.png");
                    activeListItem.key =  i + 1;
                    DataTable dtselect = new DataTable();
                    if (dt1.Rows[i]["active_type"].ToString() == "0")
                    {
                        dtselect = activeDao.SelectActiveConsume(dt1.Rows[i]["active_id"].ToString());
                    }
                    else
                    {
                        dtselect = activeDao.SelectActiveCheck(dt1.Rows[i]["active_id"].ToString());
                    }
                    if (dtselect.Rows.Count > 0)
                    {
                        if (dtselect.Rows[0][0].ToString() == "0")
                        {
                            activeListItem.img[0] = "http://llwell-wxapp.oss-cn-beijing.aliyuncs.com/A-cubic/goodsred.png";
                        }
                        else if (dtselect.Rows[0][0].ToString() == "1")
                        {
                            activeListItem.img[1] = "http://llwell-wxapp.oss-cn-beijing.aliyuncs.com/A-cubic/heartred.png";
                        }
                        else
                        {
                            activeListItem.img[2] = "http://llwell-wxapp.oss-cn-beijing.aliyuncs.com/A-cubic/upred.png";
                        }
                    }
                    activeListItem.title = dt1.Rows[i]["remark"].ToString();
                    activeListItem.time = dt1.Rows[i]["active_time_from"].ToString() + "~" + dt1.Rows[i]["active_time_to"].ToString();
                    activeListItem.drainage = dt.Select("member_name<>'' and active_id='"+ dt1.Rows[i]["active_id"].ToString() + "'").Length.ToString();
                    activeListItem.consumeNum= dt.Select("consume>'0' and active_id='" + dt1.Rows[i]["active_id"].ToString() + "'").Length.ToString();
                    activeListItem.newUser = dataView.ToTable(true, "member_name", "active_id").Select("member_name<>'' and active_id = '" + dt1.Rows[i]["active_id"].ToString() + "'").Length.ToString();                    
                    pageResult.list.Add(activeListItem);
                }
                pageResult.pagination.total = dt1.Rows.Count;
            }
            return pageResult;
        }

        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public MsgResult Do_AddActive(BaseApi baseApi)
        {
            MsgResult msg = new MsgResult();
            msg.msg = "";
            AddActiveParam addActiveParam = JsonConvert.DeserializeObject<AddActiveParam>(baseApi.param.ToString());
            if (addActiveParam.activeType == "" && addActiveParam.activeType == null)
            {
                msg.msg = "活动类型不能为空";
                return msg;
            }
            else if(addActiveParam.activeType == "0")
            {
                if (addActiveParam.consume == "" || addActiveParam.consume == null || !double.TryParse(addActiveParam.consume, out double d))
                {
                    msg.msg = "请填写正确钱数";
                    return msg;
                } 
            }
            if (addActiveParam.activeTime==null || addActiveParam.activeTime.Length!=2)
            {
                msg.msg = "请填写正确的活动时间";
                return msg;
            }
            if (addActiveParam.activeRemark=="" && addActiveParam.activeRemark ==null)
            {
                msg.msg = "活动标题不能为空";
                return msg;
            }
            if ((addActiveParam.heartItemValue == null || addActiveParam.heartItemValue == "") && (addActiveParam.limitItemValue == null || addActiveParam.limitItemValue == "") && (addActiveParam.list == null || addActiveParam.list.Count == 0))
            {
                msg.msg = "活动奖品不能为空";
                return msg;
            }                                     
            string shopId = Util.GetUserShopId(baseApi.token);
            ActiveDao activeDao = new ActiveDao();
            if (activeDao.InsertAddActive(addActiveParam, shopId))
            {
                DataTable dt = activeDao.SelectAddActive(addActiveParam, shopId);
                string id = dt.Rows[0][0].ToString();
                if (addActiveParam.activeType == "0")
                {
                    if (addActiveParam.heartItemValue != null && addActiveParam.heartItemValue != "")
                    {
                        addActiveParam.itemNums = "1";
                        addActiveParam.ItemValue = addActiveParam.heartItemValue;
                        addActiveParam.valueType = "1";
                        msg.type = activeDao.InsertActiveConsume(addActiveParam, id) == true ? 1 : 0;
                    }
                    if (addActiveParam.limitItemValue!=null && addActiveParam.limitItemValue !="")
                    {
                        addActiveParam.itemNums = "1";
                        addActiveParam.ItemValue = addActiveParam.limitItemValue;
                        addActiveParam.valueType = "2";
                        msg.type = activeDao.InsertActiveConsume(addActiveParam, id) == true ? 1 : 0;
                    }
                    if (addActiveParam.list != null && addActiveParam.list.Count >0)
                    {                        
                        addActiveParam.valueType = "0";
                        for (int i=0;i< addActiveParam.list.Count;i++)
                        {
                            addActiveParam.itemNums = addActiveParam.list[i].goodsNums;
                            addActiveParam.ItemValue = addActiveParam.list[i].goodsId;
                            msg.type = activeDao.InsertActiveConsume(addActiveParam, id) == true ? 1 : 0;
                        }                      
                    }
                }
                else
                {
                    if (addActiveParam.heartItemValue != null && addActiveParam.heartItemValue != "")
                    {
                        addActiveParam.itemNums = "1";
                        addActiveParam.ItemValue = addActiveParam.heartItemValue;
                        addActiveParam.valueType = "1";
                        msg.type = activeDao.InsertActiveCheck(addActiveParam, id) == true ? 1 : 0;
                    }
                    if (addActiveParam.limitItemValue != null && addActiveParam.limitItemValue != "")
                    {
                        addActiveParam.itemNums = "1";
                        addActiveParam.ItemValue = addActiveParam.limitItemValue;
                        addActiveParam.valueType = "2";
                        msg.type = activeDao.InsertActiveCheck(addActiveParam, id) == true ? 1 : 0;
                    }
                    if (addActiveParam.list != null && addActiveParam.list.Count > 0)
                    {
                        addActiveParam.valueType = "0";
                        for (int i = 0; i < addActiveParam.list.Count; i++)
                        {
                            addActiveParam.itemNums = addActiveParam.list[i].goodsNums;
                            addActiveParam.ItemValue = addActiveParam.list[i].goodsId;
                            msg.type = activeDao.InsertActiveCheck(addActiveParam, id) == true ? 1 : 0;
                        }
                    }
                    msg.type = activeDao.InsertActiveCheck(addActiveParam, id) == true ? 1 : 0;
                }
            }
            else
            {
                msg.msg = "Insert Buss_Active wrong";
            }
            return msg;
        }

        /// <summary>
        /// 勾选商品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public MsgResult Do_ChoseGoods(BaseApi baseApi)
        {
            MsgResult msg = new MsgResult();
            ChoseGoodsParam choseGoodsParam = JsonConvert.DeserializeObject<ChoseGoodsParam>(baseApi.param.ToString());
            if (choseGoodsParam.goodsId=="" && choseGoodsParam.goodsId == null)
            {
                msg.msg = "goodsId不能为空";
                return msg;
            }
            if (choseGoodsParam.type=="" && choseGoodsParam.type ==null )
            {
                msg.msg = "type不能为空";
                return msg;
            }
            
            string shopId = Util.GetUserShopId(baseApi.token);
            ActiveDao activeDao = new ActiveDao();
            if (choseGoodsParam.type == "1")
            {
                if (activeDao.InsertActiveGoods(shopId, choseGoodsParam))
                {
                    msg.type = 1;
                }
                else
                {
                    msg.msg = "添加失败";
                }
            }
            else
            {
                if (activeDao.DeleteActiveGoods(shopId, choseGoodsParam))
                {
                    msg.type = 1;
                }
                else
                {
                    msg.msg = "删除失败";
                }                
            }
            return msg;
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageResult Do_GoodsList(BaseApi baseApi)
        {
            PageResult pageResult = new PageResult();
            pageResult.list = new List<object>();            
            GoodsListParam goodsListParam = JsonConvert.DeserializeObject<GoodsListParam>(baseApi.param.ToString());
            if (goodsListParam.current==0)
            {
                goodsListParam.current = 1;
            }
            if (goodsListParam.pageSize==0)
            {
                goodsListParam.pageSize = 10;
            }
            if (goodsListParam.goodsName != null && goodsListParam.goodsName != "")
            {
                goodsListParam.goodsName = " and goods_name  like '%" + goodsListParam.goodsName + "%'";
            }
            else
            {
                goodsListParam.goodsName = "";
            }
            pageResult.pagination = new Page(goodsListParam.current, goodsListParam.pageSize);
            string shopId = Util.GetUserShopId(baseApi.token);
            ActiveDao activeDao = new ActiveDao();
            activeDao.DeleteActiveGoods(shopId);
            DataTable dt = activeDao.SelectGoods(goodsListParam);
            if (dt.Rows.Count>0)
            {
                DataTable dtchose = activeDao.SelectActiveGoods(shopId);
                int count = dtchose.Rows.Count;
                for (int i= (goodsListParam.current-1)* goodsListParam.pageSize; i< dt.Rows.Count && i< goodsListParam.current* goodsListParam.pageSize;i++)
                {
                    GoodsListItem goodsListItem = new GoodsListItem();
                    goodsListItem.key = i + 1;
                    goodsListItem.goodsName = dt.Rows[i]["goods_name"].ToString();
                    goodsListItem.goodsId = dt.Rows[i]["goods_id"].ToString();
                    goodsListItem.goodsCost= dt.Rows[i]["goods_cost"].ToString();
                    goodsListItem.goodsPrice = dt.Rows[i]["goods_price"].ToString();
                    goodsListItem.goodsNum = dt.Rows[i]["goods_stock"].ToString();
                    goodsListItem.img= dt.Rows[i]["goods_img"].ToString();
                    if (count>0)
                    {
                        goodsListItem.ifchose = dtchose.Select("goodsId='"+ goodsListItem.goodsId + "'").Length==1?1:0;
                    }
                    pageResult.list.Add(goodsListItem);
                }
            }
            pageResult.pagination.total = dt.Rows.Count;
            return pageResult;
        }

        /// <summary>
        /// 确认选择商品接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageResult Do_MakeSureGoodsList(BaseApi baseApi)
        {
            GoodsListParam goodsListParam = JsonConvert.DeserializeObject<GoodsListParam>(baseApi.param.ToString());
            if (goodsListParam.current==0)
            {
                goodsListParam.current = 1;
            }
            if (goodsListParam.pageSize == 0)
            {
                goodsListParam.pageSize = 10;
            }
            PageResult pageResult = new PageResult();
            pageResult.list = new List<object>();
            pageResult.pagination = new Page(goodsListParam.current, goodsListParam.pageSize);
            string shopId = Util.GetUserShopId(baseApi.token);
            ActiveDao activeDao = new ActiveDao();
            DataTable dt= activeDao.SelectActiveGoods(shopId);
            string barcodes = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        barcodes += " and goods_id in ( '" + dt.Rows[i]["goodsId"].ToString() + "',";
                    }
                    else if (i == (dt.Rows.Count - 1))
                    {
                        barcodes += " '" + dt.Rows[i]["goodsId"].ToString() + "') ";
                    }
                    else
                    {
                        barcodes += " '" + dt.Rows[i]["goodsId"].ToString() + "',";
                    }
                }
                DataTable dtGoods = activeDao.SelectGoods(barcodes);
                if (dtGoods.Rows.Count > 0)
                {
                    for (int j = (goodsListParam.current - 1) * goodsListParam.pageSize; j < dtGoods.Rows.Count && j < goodsListParam.current * goodsListParam.pageSize; j++)
                    {
                        GoodsListItem goodsListItem = new GoodsListItem();
                        goodsListItem.key = j + 1;
                        goodsListItem.goodsName = dtGoods.Rows[j]["goods_name"].ToString();
                        goodsListItem.goodsId = dtGoods.Rows[j]["goods_id"].ToString();
                        goodsListItem.goodsCost = dtGoods.Rows[j]["goods_cost"].ToString();
                        goodsListItem.goodsPrice = dtGoods.Rows[j]["goods_price"].ToString();
                        goodsListItem.goodsNum = dtGoods.Rows[j]["goods_stock"].ToString();
                        goodsListItem.img = dtGoods.Rows[j]["goods_img"].ToString();
                        pageResult.list.Add(goodsListItem);
                    }
                }
                pageResult.pagination.total = dtGoods.Rows.Count;
                activeDao.DeleteActiveGoods(shopId);
            }
            else
            {
                pageResult.pagination.total = 0;
            }
            return pageResult;
        }

        /// <summary>
        /// 修改商品数量接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public MsgResult Do_ChangeGoodsNum(BaseApi baseApi)
        {
            MsgResult msg = new MsgResult();
            AddActiveList addActiveList = JsonConvert.DeserializeObject<AddActiveList>(baseApi.param.ToString());
            if (addActiveList.goodsId==null || addActiveList.goodsId == "")
            {
                msg.msg = "商品号不能为空";                
            }
            if (addActiveList.goodsNums == null || addActiveList.goodsNums == "")
            {
                msg.msg = "商品数量不能为空";
            }
            if (msg.msg != null)
                return msg;
            ActiveDao activeDao = new ActiveDao();
            msg.type= activeDao.UpdateActiveGoods(addActiveList)==true?1:0;
            return msg;
        }
    }     
}
