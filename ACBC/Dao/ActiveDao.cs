﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACBC.Buss;
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
            selectBuilder.AppendFormat(ActiceSqls.INSERT_T_BUSS_ACTIVE, shopId, addActiveParam.activeType, addActiveParam.activeTime[0], addActiveParam.activeTime[1], addActiveParam.activeRemark);
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

        public DataTable SelectAddActive(AddActiveParam addActiveParam, string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.SELECT_T_BUSS_ACTIVE_BY_ACTIVE_ID,
                shopId, addActiveParam.activeType, addActiveParam.activeTime[0], addActiveParam.activeTime[1], addActiveParam.activeRemark);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
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

        public DataTable SelectActiveGoods(string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.SELECT_T_ACTIVE_GOODS_BY_SHOPID, shopId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public DataTable SelectGoods(GoodsListParam goodsListParam)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.SELECT_T_BUSS_GOODS_BY_BARCODE, goodsListParam.goodsName);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public DataTable SelectGoods(string barcodes)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(ActiceSqls.SELECT_T_BUSS_GOODS_BY_BARCODE, barcodes);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
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
