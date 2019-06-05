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
    public class ActiveDao
    {
        public DataTable ActiveList(string shopId, ActiveListParam activeListParam)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.SELECT_T_BUSS_ACTIVE_AND_T_BUSS_MEMBER_CHECK_STORE, shopId, activeListParam.title);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public DataTable SelectActiveConsume(string activeId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.SELECT_T_BUSS_ACTIVE_CONSUME_BY_ACTIVE_ID, activeId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public DataTable SelectActiveCheck(string activeId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.SELECT_T_BUSS_ACTIVE_CHECK_BY_ACTIVE_ID, activeId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public bool InsertAddActive(AddActiveParam addActiveParam,string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.INSERT_T_BUSS_ACTIVE, shopId, addActiveParam.activeType, addActiveParam.date[0], addActiveParam.date[1], addActiveParam.activeRemark);
            string select = selectBuilder.ToString();
            if (DatabaseOperationWeb.ExecuteDML(select))
            {
                return true;
            }
            else
            {
                return false;
            }           
        }

        public string SelectAddActive(AddActiveParam addActiveParam, string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.SELECT_T_BUSS_ACTIVE_BY_ACTIVE_ID,
                shopId, addActiveParam.activeType, addActiveParam.date[0], addActiveParam.date[1], addActiveParam.activeRemark);
            string select = selectBuilder.ToString();
            string id="";
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            if (dt.Rows.Count>0)
            {
                id= dt.Rows[0][0].ToString();
            }
            return id;
        }

        public bool InsertActiveCheck(AddActiveParam addActiveParam, string id)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.INSERT_T_BUSS_ACTIVE_CHECK, addActiveParam.itemNums, addActiveParam.ItemValue, id, addActiveParam.valueType);
            string select = selectBuilder.ToString();
            if (DatabaseOperationWeb.ExecuteDML(select))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool InsertActiveConsume(AddActiveParam addActiveParam, string id)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.INSERT_T_BUSS_ACTIVE_CONSUME, addActiveParam.consume, addActiveParam.itemNums, addActiveParam.ItemValue, id, addActiveParam.valueType);
            string select = selectBuilder.ToString();
            if (DatabaseOperationWeb.ExecuteDML(select))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool InsertActiveGoods(string shopId, ChoseGoodsParam  choseGoodsParam)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.INSERT_T_ACTIVE_GOODS, shopId, choseGoodsParam.goodsId);
            string select = selectBuilder.ToString();
            if (DatabaseOperationWeb.ExecuteDML(select))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteActiveGoods(string shopId, ChoseGoodsParam choseGoodsParam)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.DELETE_T_ACTIVE_GOODS_BY_SHOPID_GOODSID, shopId, choseGoodsParam.goodsId);
            string select = selectBuilder.ToString();
            if (DatabaseOperationWeb.ExecuteDML(select))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteActiveGoods(string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.DELETE_T_ACTIVE_GOODS_BY_SHOPID, shopId);
            string select = selectBuilder.ToString();
            if (DatabaseOperationWeb.ExecuteDML(select))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string  SelectActiveGoods(string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.SELECT_T_ACTIVE_GOODS_BY_SHOPID, shopId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            string barcodes = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows.Count==1)
                    {
                        barcodes= " and goods_id in ( '" + dt.Rows[i]["goodsId"].ToString() + "') ";
                    }
                    else if (i == 0)
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
            }
                return barcodes;
        }

        public PageResult SelectGoods(GoodsListParam goodsListParam,string shopId)
        {
            PageResult page = new PageResult();
            page.list = new List<object>();
            page.pagination = new Page(goodsListParam.current, goodsListParam.pageSize);
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.SELECT_T_BUSS_GOODS_BY_BARCODE, goodsListParam.goodsName);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];           
            if (dt.Rows.Count>0)
            {
                StringBuilder selectBuilder1 = new StringBuilder();
                selectBuilder1.AppendFormat(ActiceSqls.SELECT_T_ACTIVE_GOODS_BY_SHOPID, shopId);
                string select1 = selectBuilder1.ToString();
                DataTable dtchose = DatabaseOperationWeb.ExecuteSelectDS(select1, "T").Tables[0];
                for (int i = (goodsListParam.current - 1) * goodsListParam.pageSize; i < dt.Rows.Count && i < goodsListParam.current * goodsListParam.pageSize; i++)
                {
                    int count = dtchose.Rows.Count;
                    GoodsListItem goodsListItem = new GoodsListItem();
                    goodsListItem.key = i + 1;
                    goodsListItem.goodsName = dt.Rows[i]["goods_name"].ToString();
                    goodsListItem.goodsId = dt.Rows[i]["goods_id"].ToString();
                    goodsListItem.goodsCost = dt.Rows[i]["goods_cost"].ToString();
                    goodsListItem.goodsPrice = dt.Rows[i]["goods_price"].ToString();
                    goodsListItem.goodsNum = dt.Rows[i]["goods_stock"].ToString();
                    goodsListItem.img = dt.Rows[i]["goods_img"].ToString();
                    goodsListItem.goodsNums = "1";
                    if (count > 0)
                    {
                        goodsListItem.ifchose = dtchose.Select("goodsId='" + goodsListItem.goodsId + "'").Length == 1 ? 1 : 0;
                    }
                    page.list.Add(goodsListItem);
                }
            }
            page.pagination.total = dt.Rows.Count;
            return page;
        }

        public PageResult SelectGoods(string barcodes, GoodsListParam goodsListParam)
        {
            PageResult pageResult = new PageResult();
            pageResult.list = new List<object>();
            pageResult.pagination = new Page(goodsListParam.current, goodsListParam.pageSize);
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.SELECT_T_BUSS_GOODS_BY_BARCODE, barcodes);
            string select = selectBuilder.ToString();
            DataTable dtGoods = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
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
                    goodsListItem.goodsNums = "1";
                    pageResult.list.Add(goodsListItem);
                }
            }
            pageResult.pagination.total = dtGoods.Rows.Count;
            return pageResult;
        }

        public bool UpdateActiveGoods(AddActiveList addActiveList)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.UPDATE_T_ACTIVE_GOODS_BY_NUM_AND_GOODSID, addActiveList.goodsNums, addActiveList.goodsId);
            string select = selectBuilder.ToString();
            if (DatabaseOperationWeb.ExecuteDML(select))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class ActiceSqls
    {
        public const string UPDATE_T_ACTIVE_GOODS_BY_NUM_AND_GOODSID = ""
            + "UPDATE T_ACTIVE_GOODS "
            + " SET NUM='{0}' "
            + " WHERE GOODSID='{1}'";

        public const string SELECT_T_BUSS_GOODS_BY_BARCODE = ""
            + "SELECT GOODS_ID,GOODS_NAME,GOODS_COST,GOODS_PRICE,GOODS_STOCK,GOODS_IMG "
            + " FROM T_BUSS_GOODS "
            + " WHERE IF_USE='1' AND GOODS_STOCK>0 {0}";

        public const string SELECT_T_ACTIVE_GOODS_BY_SHOPID = ""
            + "SELECT GOODSID "
            + " FROM T_ACTIVE_GOODS "
            + " WHERE SHOPID='{0}' ";

        public const string DELETE_T_ACTIVE_GOODS_BY_SHOPID = ""
            + " DELETE FROM T_ACTIVE_GOODS "
            + " WHERE  SHOPID='{0}' ";

        public const string DELETE_T_ACTIVE_GOODS_BY_SHOPID_GOODSID = ""
            + " DELETE FROM T_ACTIVE_GOODS "
            + " WHERE  SHOPID='{0}' AND GOODSID='{1}'";

        public const string INSERT_T_ACTIVE_GOODS = ""
            + " INSERT INTO T_ACTIVE_GOODS(SHOPID,GOODSID) "
            + " VALUES('{0}','{1}')";

        public const string INSERT_T_BUSS_ACTIVE_CONSUME = ""
            + " INSERT INTO T_BUSS_ACTIVE_CONSUME(CONSUME,ITEM_NUMS,ITEM_VALUE,ACTIVE_ID,VALUE_TYPE) "
            + " VALUES('{0}','{1}','{2}','{3}','{4}')";

        public const string INSERT_T_BUSS_ACTIVE_CHECK = ""
            + " INSERT INTO T_BUSS_ACTIVE_CHECK(ITEM_NUMS,ITEM_VALUE,ACTIVE_ID,VALUE_TYPE) "
            + " VALUES('{0}','{1}','{2}','{3}')";

        public const string SELECT_T_BUSS_ACTIVE_BY_ACTIVE_ID = ""
            + " SELECT ACTIVE_ID "
            + " FROM T_BUSS_ACTIVE "
            + " WHERE ACTIVE_STORE='{0}' "
            + " AND ACTIVE_TYPE='{1}' "
            + " AND ACTIVE_TIME_FROM='{2}' "
            + " AND ACTIVE_TIME_TO='{3}' "
            + " AND REMARK='{4}'";

        public const string INSERT_T_BUSS_ACTIVE = ""
            + " INSERT INTO T_BUSS_ACTIVE(ACTIVE_STORE,ACTIVE_TYPE,ACTIVE_TIME_FROM,ACTIVE_TIME_TO,REMARK) "
            + " VALUES('{0}','{1}','{2}','{3}','{4}')";

        public const string SELECT_T_BUSS_ACTIVE_CHECK_BY_ACTIVE_ID = ""
            + " SELECT VALUE_TYPE "
            + " FROM T_BUSS_ACTIVE_CHECK "
            + " WHERE ACTIVE_ID='{0}' ";

        public const string SELECT_T_BUSS_ACTIVE_CONSUME_BY_ACTIVE_ID = ""
            + " SELECT VALUE_TYPE "
            + " FROM T_BUSS_ACTIVE_CONSUME "
            + " WHERE ACTIVE_ID='{0}'";

        public const string SELECT_T_BUSS_ACTIVE_AND_T_BUSS_MEMBER_CHECK_STORE = ""
            + " SELECT A.ACTIVE_TYPE,M.MEMBER_NAME,C.CONSUME,A.ACTIVE_ID,A.ACTIVE_TIME_FROM,A.ACTIVE_TIME_TO,A.REMARK "
            + " FROM T_BUSS_ACTIVE A LEFT JOIN T_BUSS_MEMBER_CHECK_STORE C "
            + " ON A.ACTIVE_STORE=C.STORE_ID  AND C.CHECK_TIME BETWEEN A.ACTIVE_TIME_FROM AND A.ACTIVE_TIME_TO "
            + " LEFT JOIN T_BASE_MEMBER M "
            + " ON M.MEMBER_ID = C.MEMBER_ID "
            + " AND M.REG_TIME BETWEEN A.ACTIVE_TIME_FROM "
            + " AND A.ACTIVE_TIME_TO "
            + " WHERE  A.ACTIVE_STORE = '{0}' {1}";
    }
}
