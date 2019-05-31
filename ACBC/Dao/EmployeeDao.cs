using ACBC.Buss;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ACBC.Framework.Database;

namespace ACBC.Dao
{
    public class EmployeeDao
    {
        public DataTable EmployeeLogon(string shopId, EmployeeLogonParam employeeLogonParam)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select store_user_name,store_user_img,store_user_sex,store_user_phone " +
                " from t_base_store_user where store_id='{0}' ", shopId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select,"T").Tables[0];
            return dt;
        }

        public DataTable CheckStoreCode(AddEmployeeParam addEmployeeParam)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select store_id " +
                " from t_buss_store_code where store_code='{0}' ", addEmployeeParam.storeCode);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public bool AddT_buss_store_code(AddEmployeeParam addEmployeeParam, string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("insert into t_buss_store_code(store_id, store_code,state)  " +
                " values('{0}','{1}','{2}')  ", addEmployeeParam.storeCode, addEmployeeParam.state, shopId);
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

        public DataTable CheckStoreId(string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select store_id " +
                " from t_buss_store_code where store_id='{0}' ", shopId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public bool UpdateT_buss_store_code(AddEmployeeParam addEmployeeParam, string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("update t_buss_store_code set store_code='{0}',state='{1}'  " +
                " where store_id='{2}' ", addEmployeeParam.storeCode, addEmployeeParam.state, shopId);
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

        public DataTable CheckOldStoreId(string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat("select store_code,state " +
                " from t_buss_store_code where store_id='{0}' ", shopId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }
    }
}
