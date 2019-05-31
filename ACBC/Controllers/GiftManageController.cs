using ACBC.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Controllers
{
    [Produces("application/json")]
    [Consumes("multipart/form-data", "application/json")]
    [Route(Global.ROUTE_PX + "/[controller]/[action]")]
    [EnableCors("AllowSameDomain")]
    public class GiftManageController : Controller
    {
        private IHttpContextAccessor _accessor;

        public GiftManageController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        /// <summary>
        /// 商品管理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Goods([FromBody]GoodsApi goodsApi)
        {
            if (goodsApi == null)
                return Json(new ResultsJson(new Message(CodeMessage.PostNull, "PostNull"), null));
            return Json(Global.BUSS.BussResults(this, goodsApi, _accessor));
        }

        /// <summary>
        /// 用户管理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult User([FromBody]UserApi userApi)
        {
            if (userApi == null)
                return Json(new ResultsJson(new Message(CodeMessage.PostNull, "PostNull"), null));
            return Json(Global.BUSS.BussResults(this, userApi, _accessor));
        }

        /// <summary>
        /// 订单管理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Order([FromBody]OrderApi orderApi)
        {
            if (orderApi == null)
                return Json(new ResultsJson(new Message(CodeMessage.PostNull, "PostNull"), null));
            return Json(Global.BUSS.BussResults(this, orderApi, _accessor));
        }

        /// <summary>
        /// 店员管理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Employee([FromBody]EmployeeApi employeeApi)
        {
            if (employeeApi == null)
                return Json(new ResultsJson(new Message(CodeMessage.PostNull, "PostNull"), null));
            return Json(Global.BUSS.BussResults(this, employeeApi, _accessor));
        }

        /// <summary>
        /// 会员管理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Member([FromBody]MemberApi memberApi)
        {
            if (memberApi == null)
                return Json(new ResultsJson(new Message(CodeMessage.PostNull, "PostNull"), null));
            return Json(Global.BUSS.BussResults(this, memberApi, _accessor));
        }

        /// <summary>
        /// 活动管理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Active([FromBody]ActiveApi activeApi)
        {
            if (activeApi == null)
                return Json(new ResultsJson(new Message(CodeMessage.PostNull, "PostNull"), null));
            return Json(Global.BUSS.BussResults(this, activeApi, _accessor));
        }
    }
}
