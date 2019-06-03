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
    public class MemberBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.MemberApi;
        }

        /// <summary>
        /// 会员列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageResult Do_MemberList(BaseApi baseApi)
        {
            PageResult pageResult = new PageResult();
            pageResult.list = new List<object>();
            MemberListParam memberListParam = JsonConvert.DeserializeObject<MemberListParam>(baseApi.param.ToString());
            if (memberListParam.current == 0)
            {
                memberListParam.current = 1;
            }
            if (memberListParam.pageSize==0)
            {
                memberListParam.pageSize = 10;
            }
            if (memberListParam.userName != "" && memberListParam.userName != null)
            {
                memberListParam.userName = " and m.member_name='" + memberListParam.userName + "'";
            }
            else
            {
                memberListParam.userName = "";
            }
            if (memberListParam.sex != "" && memberListParam.sex != null)
            {
                memberListParam.sex = memberListParam.sex == "男" ? "1" : "2";
                memberListParam.sex = " and m.member_sex='" + memberListParam.sex + "'";
            }
            else
            {
                memberListParam.sex = "";
            }
            pageResult.pagination = new Page(memberListParam.current, memberListParam.pageSize);          
            string shopId = Util.GetUserShopId(baseApi.token);
            MemberDao memberDao = new MemberDao();
            DataTable dt= memberDao.MemberList(shopId, memberListParam);
            if (dt.Rows.Count>0)
            {
                for (int i=(memberListParam.current - 1)* memberListParam.pageSize;i< dt.Rows.Count && i< memberListParam.current * memberListParam.pageSize;i++)
                {
                    MemberListItem memberListItem = new MemberListItem();
                    memberListItem.key = i+1;
                    memberListItem.name = dt.Rows[i]["member_name"].ToString();
                    memberListItem.phone= dt.Rows[i]["reg_phone"].ToString();
                    memberListItem.img = dt.Rows[i]["member_img"].ToString();
                    memberListItem.sex = dt.Rows[i]["member_sex"].ToString()=="1"?"男":"女";
                    pageResult.list.Add(memberListItem);
                }
            }
            pageResult.pagination.total = dt.Rows.Count;
            return pageResult;
        }
        
    }  
}
