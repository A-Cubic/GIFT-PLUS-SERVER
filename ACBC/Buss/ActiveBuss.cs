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
        public string Do_AddActive(BaseApi baseApi)
        {
            AddActiveParam addActiveParam = JsonConvert.DeserializeObject<AddActiveParam>(baseApi.param.ToString());
            if (addActiveParam.activeType == "" && addActiveParam.activeType == null)
            {
                throw new ApiException(CodeMessage.InvalidActiveType, "InvalidActiveType");
            }
            else if(addActiveParam.activeType == "0")
            {
                if (addActiveParam.consume == "" || addActiveParam.consume == null || !double.TryParse(addActiveParam.consume, out double d))
                {
                    throw new ApiException(CodeMessage.InvalidConsume, "InvalidConsume");
                } 
            }
            if (addActiveParam.activeTime == null || addActiveParam.activeTime.Length != 2)
            {
                throw new ApiException(CodeMessage.InvalidTime, "InvalidTime");
            }
            else
            {
                addActiveParam.activeTime[0] = Convert.ToDateTime(addActiveParam.activeTime[0].ToString("yyyy-MM-dd hh:mm:ss")) ;
                addActiveParam.activeTime[1] = Convert.ToDateTime(addActiveParam.activeTime[1].ToString("yyyy-MM-dd hh:mm:ss"));
            }
            if (addActiveParam.activeRemark=="" && addActiveParam.activeRemark ==null)
            {
                throw new ApiException(CodeMessage.InvalidRemark, "InvalidRemark");
            }
            if ((addActiveParam.heartItemValue == null || addActiveParam.heartItemValue == "") && (addActiveParam.limitItemValue == null || addActiveParam.limitItemValue == "") && (addActiveParam.list == null || addActiveParam.list.Count == 0))
            {
                throw new ApiException(CodeMessage.InvalidGift, "InvalidGift");
            }
            if (addActiveParam.list!=null)
            {
                for (int i=0;i< addActiveParam.list.Count;i++)
                {
                    if (addActiveParam.list[i].goodsId==null || addActiveParam.list[i].goodsId =="")
                    {
                        throw new ApiException(CodeMessage.InvalidGoodsIdCode, "InvalidGoodsIdCode");
                    }
                    if (addActiveParam.list[i].goodsNums == null || addActiveParam.list[i].goodsNums == "")
                    {
                        throw new ApiException(CodeMessage.InvalidGoodsNumCode, "InvalidGoodsNumCode");
                    }
                }                
            }
            string shopId = Util.GetUserShopId(baseApi.token);
            ActiveDao activeDao = new ActiveDao();
            if (activeDao.InsertAddActive(addActiveParam, shopId))
            {
                string id = activeDao.SelectAddActive(addActiveParam, shopId);
                 
                if (addActiveParam.activeType == "0")
                {
                    if (addActiveParam.heartItemValue != null && addActiveParam.heartItemValue != "")
                    {
                        addActiveParam.itemNums = "1";
                        addActiveParam.ItemValue = addActiveParam.heartItemValue;
                        addActiveParam.valueType = "1";
                        if (!activeDao.InsertActiveConsume(addActiveParam, id))
                        {
                            throw new ApiException(CodeMessage.DBAddError, "DBAddError");
                        }
                    }
                    if (addActiveParam.limitItemValue!=null && addActiveParam.limitItemValue !="")
                    {
                        addActiveParam.itemNums = "1";
                        addActiveParam.ItemValue = addActiveParam.limitItemValue;
                        addActiveParam.valueType = "2";
                        if (!activeDao.InsertActiveConsume(addActiveParam, id))
                        {
                            throw new ApiException(CodeMessage.DBAddError, "DBAddError");
                        }
                    }
                    if (addActiveParam.list != null && addActiveParam.list.Count >0)
                    {                        
                        addActiveParam.valueType = "0";
                        for (int i=0;i< addActiveParam.list.Count;i++)
                        {
                            addActiveParam.itemNums = addActiveParam.list[i].goodsNums;
                            addActiveParam.ItemValue = addActiveParam.list[i].goodsId;
                            if (!activeDao.InsertActiveConsume(addActiveParam, id))
                            {
                                throw new ApiException(CodeMessage.DBAddError, "DBAddError");
                            }
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
                        if (!activeDao.InsertActiveCheck(addActiveParam, id))
                        {
                            throw new ApiException(CodeMessage.DBAddError, "DBAddError");
                        }
                    }
                    if (addActiveParam.limitItemValue != null && addActiveParam.limitItemValue != "")
                    {
                        addActiveParam.itemNums = "1";
                        addActiveParam.ItemValue = addActiveParam.limitItemValue;
                        addActiveParam.valueType = "2";
                        if (!activeDao.InsertActiveCheck(addActiveParam, id))
                        {
                            throw new ApiException(CodeMessage.DBAddError, "DBAddError");
                        }
                    }
                    if (addActiveParam.list != null && addActiveParam.list.Count > 0)
                    {
                        addActiveParam.valueType = "0";
                        for (int i = 0; i < addActiveParam.list.Count; i++)
                        {
                            addActiveParam.itemNums = addActiveParam.list[i].goodsNums;
                            addActiveParam.ItemValue = addActiveParam.list[i].goodsId;
                            if (!activeDao.InsertActiveCheck(addActiveParam, id))
                            {
                                throw new ApiException(CodeMessage.DBAddError, "DBAddError");
                            }
                        }
                    }
                    if (!activeDao.InsertActiveCheck(addActiveParam, id))
                    {
                        throw new ApiException(CodeMessage.DBAddError, "DBAddError");
                    }
                }
            }
            else
            {
                throw new ApiException(CodeMessage.DBAddError, "DBAddError");
            }
            activeDao.DeleteActiveGoods(shopId);
            return "";
        }

        /// <summary>
        /// 勾选商品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string Do_ChoseGoods(BaseApi baseApi)
        {
            MsgResult msg = new MsgResult();
            ChoseGoodsParam choseGoodsParam = JsonConvert.DeserializeObject<ChoseGoodsParam>(baseApi.param.ToString());
            if (choseGoodsParam.goodsId=="" || choseGoodsParam.goodsId == null)
            {
                throw new ApiException(CodeMessage.InvalidGoodsIdCode, "InvalidGoodsIdCode");
            }
            
            string shopId = Util.GetUserShopId(baseApi.token);
            ActiveDao activeDao = new ActiveDao();
            if (choseGoodsParam.type)
            {
                if (!activeDao.InsertActiveGoods(shopId, choseGoodsParam))
                {
                    throw new ApiException(CodeMessage.DBAddError, "DBAddError");
                }
            }
            else
            {
                if (!activeDao.DeleteActiveGoods(shopId, choseGoodsParam))
                {
                    throw new ApiException(CodeMessage.DBDelError, "DBDelError");
                }                
            }
            return "";
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageResult Do_GoodsList(BaseApi baseApi)
        {
            PageResult pageResult = new PageResult();
            ActiveDao activeDao = new ActiveDao();
            string shopId = Util.GetUserShopId(baseApi.token);
            GoodsListParam goodsListParam = JsonConvert.DeserializeObject<GoodsListParam>(baseApi.param.ToString());
            if (goodsListParam.current==0)
            {
                goodsListParam.current = 1;
            }
            if (goodsListParam.pageSize==0)
            { 
                activeDao.DeleteActiveGoods(shopId);
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
            pageResult = activeDao.SelectGoods(goodsListParam, shopId);          
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
            string shopId = Util.GetUserShopId(baseApi.token);
            ActiveDao activeDao = new ActiveDao();
            string barcodes = activeDao.SelectActiveGoods(shopId);
            if (barcodes!="")
            {
                pageResult = activeDao.SelectGoods(barcodes, goodsListParam);               
                //activeDao.DeleteActiveGoods(shopId);
            }            
            return pageResult;
        }

        /// <summary>
        /// 修改商品数量接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string Do_ChangeGoodsNum(BaseApi baseApi)
        {
            AddActiveList addActiveList = JsonConvert.DeserializeObject<AddActiveList>(baseApi.param.ToString());
            if (addActiveList.goodsId==null || addActiveList.goodsId == "")
            {
                throw new ApiException(CodeMessage.InvalidGoodsIdCode, "InvalidGoodsIdCode");              
            }
            if (addActiveList.goodsNums == null || addActiveList.goodsNums == "")
            {
                throw new ApiException(CodeMessage.InvalidGoodsNumCode, "InvalidGoodsNumCode");
            }
            ActiveDao activeDao = new ActiveDao();
            if (!activeDao.UpdateActiveGoods(addActiveList))
            {
                throw new ApiException(CodeMessage.DBUpdateError, "DBUpdateError");
            }
            return ""; 
        }
    }     
}
