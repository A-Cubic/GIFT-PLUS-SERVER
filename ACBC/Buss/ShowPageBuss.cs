using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ACBC.Common;
using ACBC.Dao;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ACBC.Buss
{
    public class ShowPageBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.ShowPageApi;
        }

        public SimpleShowPageItem Do_SimpleShowPage(BaseApi baseApi)
        {
            ShowPageDao showPageDao = new ShowPageDao();
            SimpleShowPageParam simpleShowPageParam = JsonConvert.DeserializeObject<SimpleShowPageParam>(baseApi.param.ToString());
            if (simpleShowPageParam.equipmentId==null || simpleShowPageParam.equipmentId=="")
            {
                throw new ApiException(CodeMessage.ErrorEquipmentId, "分组参数错误");
            }
            SimpleShowPageItem simpleShowPageItem = new SimpleShowPageItem();
            simpleShowPageItem.banners = showPageDao.SimpleShowPageBanner(simpleShowPageParam.equipmentId);
            simpleShowPageItem.menu = showPageDao.SimpleShowPageMenu(simpleShowPageParam.equipmentId);
            simpleShowPageItem.button = showPageDao.SimpleShowPageButton(simpleShowPageParam.equipmentId);
            return simpleShowPageItem;
        }

        public SimpleShowPageSignItem Do_SimpleShowPageSign(BaseApi baseApi)
        {            
            SimpleShowPageSignItem simpleShowPageSignItem = new SimpleShowPageSignItem();
            simpleShowPageSignItem.appId = Global.AppId;
            string url = "";
            string contentType = "";
            string rep = "";
            string access_token=Util.GetRedis("access_token");
            if (access_token==null || access_token =="")
            {
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&"
                + "appid="+ Global.AppId + "&secret="+ Global.Secret ;
                contentType = "application / json";
                rep = Util.HttpGet(url, contentType);
                TokenItem tokenItem = JsonConvert.DeserializeObject<TokenItem>(rep);
                Util.SaveRedis("access_token", tokenItem.access_token, 7000);
                access_token = tokenItem.access_token;
            }
            //Util.DeleteRedis("jsapi_ticket");
            string jsapi_ticket= Util.GetRedis("jsapi_ticket");
            if (jsapi_ticket==null || jsapi_ticket=="")
            {
                url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + access_token + "&type=jsapi";
                rep = Util.HttpGet(url, contentType);
                TicketItem ticketItem = JsonConvert.DeserializeObject<TicketItem>(rep);
                Util.SaveRedis("jsapi_ticket", ticketItem.ticket, 7000);
                jsapi_ticket = ticketItem.ticket;
            }
            
            string nonceStr = Guid.NewGuid().ToString("N");
            string timestamp = DateTime.Now.ToString("yyyyMMddhh");
            string signature = "jsapi_ticket=" + jsapi_ticket
                             + "&noncestr=" + nonceStr
                             + "&timestamp="+ timestamp                             
                             + "&url=http://www.a-cubic.com/wx/wwj";            
            signature = Util.Sha1(signature);
            simpleShowPageSignItem.nonceStr = nonceStr;
            simpleShowPageSignItem.signature = signature;
            simpleShowPageSignItem.timestamp = timestamp;
            return simpleShowPageSignItem;
        }
              
    }
}
