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
    public class EmployeeBuss : IBuss
    {
        public ApiType GetApiType()
        {
           return ApiType.EmployeeApi;
        }

        /// <summary>
        /// 店员列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageResult Do_EmployeeLogon(BaseApi baseApi)
        {
            PageResult pageResult = new PageResult();
            EmployeeLogonParam employeeLogonParam = JsonConvert.DeserializeObject<EmployeeLogonParam>(baseApi.param.ToString());
            if (employeeLogonParam.current == 0)
            {
                employeeLogonParam.current = 1;
            }
            if (employeeLogonParam.pageSize==0)
            {
                employeeLogonParam.pageSize = 10;
            }
            pageResult.pagination = new Page(employeeLogonParam.current, employeeLogonParam.pageSize);
            string shopId = Util.GetUserShopId(baseApi.token);
            string power = Util.GetUserPower(baseApi.token);
            if (power!="1")
            {               
                throw new ApiException(CodeMessage.InsufficientAuthority, "InsufficientAuthority"); 
            }
            EmployeeDao employeeDao = new EmployeeDao();
            pageResult  = employeeDao.EmployeeLogon(shopId, employeeLogonParam);                       
            return pageResult;
        }

        /// <summary>
        /// 新增店员
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string Do_AddEmployee(BaseApi baseApi)
        {
            EmployeeDao employeeDao = new EmployeeDao();
            AddEmployeeParam addEmployeeParam = JsonConvert.DeserializeObject<AddEmployeeParam>(baseApi.param.ToString());
            string shopId = Util.GetUserShopId(baseApi.token);
            string power = Util.GetUserPower(baseApi.token);
            string userId = Util.GetUserUserId(baseApi.token);
            if (power != "1")
            {
                throw new ApiException(CodeMessage.InsufficientAuthority, "InsufficientAuthority");
            }
            if (addEmployeeParam.storeCode == null || addEmployeeParam.storeCode == "" || addEmployeeParam.storeCode.Length != 4)
            {
                throw new ApiException(CodeMessage.CodeError, "CodeError");
            }
            else
            {
                DataTable dt1 = employeeDao.CheckStoreCode(addEmployeeParam);
                if (dt1.Rows.Count > 0)
                {
                    if (shopId != dt1.Rows[0][0].ToString())                             
                    {
                        throw new ApiException(CodeMessage.CodeRepeat, "CodeRepeat");
                    }
                }                                
            }           
            if (addEmployeeParam.state == null || addEmployeeParam.state == "" || !int.TryParse(addEmployeeParam.state,out int i) )
            {
                throw new ApiException(CodeMessage.ErrorLogonNum, "ErrorLogonNum");              
            }                                              
            
            DataTable dt = employeeDao.CheckStoreId(shopId);
            if (dt.Rows.Count == 1)
            {
                if (!employeeDao.UpdateT_buss_store_code(addEmployeeParam, shopId))             
                {
                    throw new ApiException(CodeMessage.InterfaceDBError, "InterfaceDBError");
                }
            }
            else
            {
                if (!employeeDao.AddT_buss_store_code(addEmployeeParam, shopId))
                {
                    throw new ApiException(CodeMessage.DBAddError, "DBAddError");
                }
            }            
            return "";
        }

        /// <summary>
        /// 新增店员检查用户名
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public AddEmployeeParam Do_CheckOldStoreCode(BaseApi baseApi)
        {                      
            string shopId = Util.GetUserShopId(baseApi.token);
            EmployeeDao employeeDao = new EmployeeDao();
            AddEmployeeParam msg = employeeDao.CheckOldStoreId(shopId);           
           
            return msg;
        }
    }

  
}
