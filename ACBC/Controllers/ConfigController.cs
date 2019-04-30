using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACBC.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ACBC.Controllers
{
    [Produces("application/json")]
    [Route(Global.ROUTE_PX + "/[controller]/[action]")]
    public class ConfigController : Controller
    {
        private IHttpContextAccessor _accessor;

        public ConfigController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        [HttpPost]
        public ActionResult Open([FromBody]OpenApi openApi)
        {
            if (openApi == null)
                return Json(new ResultsJson(new Message(CodeMessage.PostNull, "PostNull"), null));
            return Json(Global.BUSS.BussResults(this, openApi, _accessor));
        }
    }
}