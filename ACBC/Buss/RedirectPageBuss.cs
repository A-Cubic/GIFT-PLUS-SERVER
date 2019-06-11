using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACBC.Dao;

namespace ACBC.Buss
{
    public class RedirectPageBuss
    {
        public string RedirectPage(string code)
        {
            RedirectPageDao redirectPageDao = new RedirectPageDao();
            string page = redirectPageDao.RedirectPage(code);
            return page;
        }
    }
}
