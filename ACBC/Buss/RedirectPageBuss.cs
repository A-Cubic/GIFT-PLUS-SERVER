using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACBC.Common;
using ACBC.Dao;

namespace ACBC.Buss
{
    public class RedirectPageBuss
    {
        public string RedirectPage(string code)
        {
            string page= Util.GetRedis(code);
            if(page==null || page=="" )
            {
                RedirectPageDao redirectPageDao = new RedirectPageDao();
                page = redirectPageDao.RedirectPage(code);
                Util.SaveRedis(code, page, 300);
            }            
            return page;
        }
    }
}
