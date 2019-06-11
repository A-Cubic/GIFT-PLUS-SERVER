using ACBC.Buss;
using ACBC.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Controllers
{
    [Route(Global.ROUTE_PX+"/[controller]/[action]")]
    public class RedirectPageController: Controller
    {
        [HttpGet]
        [ActionName("RedirectPage")]
        public ActionResult RedirectPage(string code)
        {
            RedirectPageBuss redirectPageBuss = new RedirectPageBuss();
            string page = redirectPageBuss.RedirectPage(code);
            return Redirect(page);
        }

        
    }
}
