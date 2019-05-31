using System;
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
            selectBuilder.AppendFormat("select a.active_type,m.member_name,c.consume,a.active_id,a.active_time_from,a.active_time_to,a.remark from t_buss_active a LEFT JOIN t_buss_member_check_store c on a.active_store=c.store_id  and c.check_time BETWEEN a.active_time_from and a.active_time_to "
            + " LEFT JOIN t_base_member m on m.member_id = c.member_id and m.reg_time BETWEEN a.active_time_from and a.active_time_to "
            + " where  a.active_store = '{0}' {1} ", shopId, activeListParam.title);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public DataTable Select_T_Buss_Active_Consume(string activeId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select value_type from t_buss_active_consume where active_id='{0}'", activeId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public DataTable Select_T_Buss_Active_Check(string activeId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select value_type from t_buss_active_check where active_id='{0}'", activeId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public bool Insert_T_Buss_Active(AddActiveParam addActiveParam,string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("insert into t_buss_active(active_store,active_type,active_time_from,active_time_to,remark)" +
                " values('{0}','{1}','{2}','{3}','{4}')", shopId, addActiveParam.activeType, addActiveParam.activeTime[0], addActiveParam.activeTime[1], addActiveParam.activeRemark);
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

        public DataTable Select_T_Buss_Active_ID(AddActiveParam addActiveParam, string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select active_id from t_buss_active where active_store='{0}' and active_type='{1}' and active_time_from='{2}' and active_time_to='{3}' and remark='{4}'",
                shopId, addActiveParam.activeType, addActiveParam.activeTime[0], addActiveParam.activeTime[1], addActiveParam.activeRemark);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public bool Insert_T_Buss_Active_Check(AddActiveParam addActiveParam, string id)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("insert into t_buss_active_check(item_nums,item_value,active_id,value_type)" +
                " values('{0}','{1}','{2}','{3}')", addActiveParam.itemNums, addActiveParam.ItemValue, id, addActiveParam.valueType);
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

        public bool Insert_T_Buss_Active_Consume(AddActiveParam addActiveParam, string id)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("insert into t_buss_active_consume(consume,item_nums,item_value,active_id,value_type)" +
                " values('{0}','{1}','{2}','{3}','{4}')", addActiveParam.consume, addActiveParam.itemNums, addActiveParam.ItemValue, id, addActiveParam.valueType);
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

        public bool Insert_T_Active_Goods(string shopId, ChoseGoodsParam  choseGoodsParam)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("insert into t_active_goods(shopId,goodsId)" +
                " values('{0}','{1}')", shopId, choseGoodsParam.goodsId);
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

        public bool Delete_T_Active_Goods(string shopId, ChoseGoodsParam choseGoodsParam)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("delete from t_active_goods " +
                " where  shopId='{0}' and goodsId='{1}'", shopId, choseGoodsParam.goodsId);
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

        public bool Delete_T_Active_Goods(string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("delete from t_active_goods " +
                " where  shopId='{0}' ", shopId);
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

        public DataTable Select_T_Active_Goods(string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select goodsId " +
                "from t_active_goods " +
                "where shopId='{0}' ", shopId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public DataTable Select_T_Buss_Goods(GoodsListParam goodsListParam)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select goods_id,goods_name,goods_cost,goods_price,goods_stock,goods_img " +
                "from t_buss_goods " +
                "where if_use='1' and goods_stock>0 {0} ", goodsListParam.goodsName);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public DataTable Select_T_Buss_Goods(string barcodes)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select goods_id,goods_name,goods_cost,goods_price,goods_stock,goods_img " +
                "from t_buss_goods " +
                "where if_use='1' and goods_stock>0 {0} ", barcodes);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public bool Update_T_Active_Goods(AddActiveList addActiveList)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("update t_active_goods set num='{0}' " +
                "where goodsId='{1}' ", addActiveList.goodsNums, addActiveList.goodsId);
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
}
