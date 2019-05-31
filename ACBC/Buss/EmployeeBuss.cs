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
            pageResult.list = new List<object>();
            MsgResult msgResult = new MsgResult();
            List<UserMessage> list = Util.GetUserMessage(baseApi.token);
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
            string shopId = list[0].shopId;
            string power = list[0].power;
            if (power!="1")
            {               
                msgResult.msg = "账号权限不足，无法查看";
                pageResult.item = msgResult;
                return pageResult;
            }
            EmployeeDao employeeDao = new EmployeeDao();
            DataTable dt = employeeDao.EmployeeLogon(shopId, employeeLogonParam);
            if (dt.Rows.Count>0)
            {
                for (int i= (employeeLogonParam.current - 1)* employeeLogonParam.pageSize;i<dt.Rows.Count && i< employeeLogonParam.current * employeeLogonParam.pageSize;i++)
                {
                    EmployeeLogonItem employeeLogonItem = new EmployeeLogonItem();
                    employeeLogonItem.key =  i + 1;
                    employeeLogonItem.img= dt.Rows[i]["store_user_img"].ToString();
                    employeeLogonItem.userName = dt.Rows[i]["store_user_name"].ToString();
                    employeeLogonItem.phone = dt.Rows[i]["store_user_phone"].ToString();
                    employeeLogonItem.sex = dt.Rows[i]["store_user_sex"].ToString()=="1"?"男":"女";
                    pageResult.list.Add(employeeLogonItem);
                }
            }
            pageResult.pagination.total = dt.Rows.Count;
            msgResult.type = 1;
            pageResult.item = msgResult;
            return pageResult;
        }

        /// <summary>
        /// 新增店员
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public MsgResult Do_AddEmployee(BaseApi baseApi)
        {
            MsgResult msg = new MsgResult();
            EmployeeDao employeeDao = new EmployeeDao();
            AddEmployeeParam addEmployeeParam = JsonConvert.DeserializeObject<AddEmployeeParam>(baseApi.param.ToString());
            List<UserMessage> list = Util.GetUserMessage(baseApi.token);
            string shopId = list[0].shopId;
            string power = list[0].power;
            string userId = list[0].userId;
            if (power != "1")
            {
                msg.msg = "账号权限不足，无法创建";               
                return msg;
            }
            if (addEmployeeParam.storeCode == null || addEmployeeParam.storeCode == "" || addEmployeeParam.storeCode.Length != 4)
            {
                msg.msg = "请输入正确的验证码";
                return msg;
            }
            else
            {
                DataTable dt1 = employeeDao.CheckStoreCode(addEmployeeParam);
                if (dt1.Rows.Count > 0)
                {
                    if (shopId != dt1.Rows[0][0].ToString())                             
                    {
                        msg.msg = "验证码已存在";
                        return msg;
                    }
                }                                
            }           
            if (addEmployeeParam.state == null || addEmployeeParam.state == "" || !int.TryParse(addEmployeeParam.state,out int i) )
            {
                msg.msg = "请输入正确的可注册数量";
                return msg;
            }                                              
            
            DataTable dt = employeeDao.CheckStoreId(shopId);
            if (dt.Rows.Count == 1)
            {
                if (employeeDao.UpdateT_buss_store_code(addEmployeeParam, shopId))
                {
                    msg.type = 1;
                }
                else
                {
                    msg.msg = "修改失败";
                }
            }
            else
            {
                if (employeeDao.AddT_buss_store_code(addEmployeeParam, shopId))
                {
                    msg.type = 1;
                }
                else
                {
                    msg.msg = "创建失败";
                }
            }            
            return msg;
        }

        /// <summary>
        /// 新增店员检查用户名
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public AddEmployeeParam Do_CheckOldStoreCode(BaseApi baseApi)
        {
            AddEmployeeParam msg = new AddEmployeeParam();           
            List<UserMessage> list = Util.GetUserMessage(baseApi.token);
            string shopId = list[0].shopId;
            EmployeeDao employeeDao = new EmployeeDao();
            DataTable dt = employeeDao.CheckOldStoreId(shopId);
            if (dt.Rows.Count > 0  )
            {
                msg.storeCode = dt.Rows[0]["store_code"].ToString();
                msg.state= dt.Rows[0]["state"].ToString();
            }
            
            return msg;
        }
    }

    public class AddEmployeeParam
    {
        public string storeCode;//验证码
        public string state;//可注册数量
    }

    public class EmployeeLogonParam
    {
        public int current;
        public int pageSize;
    }

    public class EmployeeLogonItem
    {
        public int key;
        public string img;//图片
        public string userName;//用户名
        public string sex;//性别
        public string phone;//电话
    }
}
