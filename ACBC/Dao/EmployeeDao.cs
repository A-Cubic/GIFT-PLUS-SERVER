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
            selectBuilder.AppendFormat(EmployeeSql.SELECT_T_BASE_STORE_USER_BY_STORE_ID, shopId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select,"T").Tables[0];
            return dt;
        }

        public DataTable CheckStoreCode(AddEmployeeParam addEmployeeParam)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(EmployeeSql.SELECT_T_BUSS_STORE_CODE_BY_STORE_CODE, addEmployeeParam.storeCode);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public bool AddT_buss_store_code(AddEmployeeParam addEmployeeParam, string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(EmployeeSql.INSERT_T_BUSS_STORE_CODE, addEmployeeParam.storeCode, addEmployeeParam.state, shopId);
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
            selectBuilder.AppendFormat(EmployeeSql.SELECT_T_BUSS_STORE_CODE, shopId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }

        public bool UpdateT_buss_store_code(AddEmployeeParam addEmployeeParam, string shopId)
        {
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendFormat(EmployeeSql.UPDATE_T_BUSS_STORE_CODE, addEmployeeParam.storeCode, addEmployeeParam.state, shopId);
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
            selectBuilder.AppendFormat(EmployeeSql.SELECT_T_BUSS_STORE_CODE_BY_STORE_ID, shopId);
            string select = selectBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            return dt;
        }
    }

    public class EmployeeSql
    {
        public const string SELECT_T_BUSS_STORE_CODE_BY_STORE_ID = ""
            + " SELECT STORE_CODE,STATE   "
            + " FROM T_BUSS_STORE_CODE "
            + " WHERE STORE_ID='{0}'";

        public const string UPDATE_T_BUSS_STORE_CODE = ""
            + " UPDATE T_BUSS_STORE_CODE "
            + " SET STORE_CODE='{0}',"
            + " STATE='{1}' "
            + " WHERE STORE_ID='{2}'";

        public const string SELECT_T_BUSS_STORE_CODE = ""
            + " SELECT STORE_ID  "
            + " FROM T_BUSS_STORE_CODE "
            + " WHERE STORE_ID='{0}'";

        public const string INSERT_T_BUSS_STORE_CODE = ""
            + " INSERT INTO T_BUSS_STORE_CODE(STORE_ID, STORE_CODE,STATE)  "
            + " VALUES('{0}','{1}','{2}')";

        public const string SELECT_T_BUSS_STORE_CODE_BY_STORE_CODE = ""
            + " SELECT STORE_ID  "
            + " FROM T_BUSS_STORE_CODE "
            + " WHERE STORE_CODE='{0}'";

        public const string SELECT_T_BASE_STORE_USER_BY_STORE_ID = ""
            + " SELECT STORE_USER_NAME,STORE_USER_IMG,STORE_USER_SEX,STORE_USER_PHONE  "
            + " FROM T_BASE_STORE_USER "
            + " WHERE STORE_ID='{0}'";
    }
}
