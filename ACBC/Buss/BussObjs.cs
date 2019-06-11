using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Buss
{

    #region Sys
    public class GetConfigParam
    {
        public string env;
        public string group;
    }

    public class UpdateConfigParam
    {
        public string env;
        public string group;
    }

    public class RequestParam
    {
        public string method;
        public object param;
    }

    public class ConfigGroup
    {
        public string key;
        public List<ConfigItem> list = new List<ConfigItem>();
        public ConfigGroup(string key)
        {
            this.key = key;
        }
    }

    public class ResponseObj
    {
        public bool success;
        public ResponseMsg msg;
        public List<ConfigItem> data;
    }

    public class ResponseMsg
    {
        public string code;
        public string msg;
    }

    public class ConfigItem
    {
        public string key;
        public string value;
    }

    #endregion

    #region Param
    public class SimpleShowPageSignParam
    {
        public string url;
    }

    public class GoodsListParam
    {
        public string goodsName;
        public int current;
        public int pageSize;
    }

    public class ChoseGoodsParam
    {
        public string goodsId;// 
        public bool type;//false删除，true添加
    }

    public class AddActiveParam
    {
        public string activeType;//活动类型
        public string[] date;//活动时间
        public string activeRemark;//活动标题
        public string consume;//钱数
        public string heartItemValue;//心值
        public string limitItemValue;//提高上限值
        public string ItemValue;
        public string itemNums;
        public string valueType;
        public List<AddActiveList> list;
    }

    public class AddActiveList
    {
        public string goodsNums;//商品数量
        public string goodsId;//商品  
    }

    public class ActiveListParam
    {
        public string title;
        public int current;
        public int pageSize;
    }

    public class ActiveOperationParam
    {
        public string activeId;
        public string operation;//操作
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

    public class StockStatisticsParam
    {
        public string status;//状态
        public int current = 1;//页数
        public int pageSize = 10;//一页几个
    }

    public class MemberListParam
    {
        public string userName;
        public string sex;
        public int current;
        public int pageSize;
    }

    public class OrderListParam
    {
        public string orderCode;//订单code
        public string[] date;//时间段  
        public string state;//状态
        public int current;
        public int pageSize;
    }

    public class OrderDetailsParam
    {
        public string orderCode;//订单code
        public int current;
        public int pageSize;
    }

    public class UserLoginParam
    {
        public string userName;//用户名
        public string password;//密码
    }
    #endregion

    #region Item
    public class SimpleShowPageSignItem
    {
        public string appId;
        public string timestamp;
        public string nonceStr;
        public string signature;
    }

    public class TicketItem
    {
        public int errcode;
        public string errmsg;
        public string ticket;
        public int expires_in;
    }

    public class TokenItem
    {
        public string access_token;
        public string expires_in;
        public string errcode;
        public string errmsg;
    }

    public class SimpleShowPageItem
    {
        public List<SimpleShowPageList> banners;
        public List<SimpleShowPageList> menu;
        public SimpleShowPageList button;
    }

    public class SimpleShowPageList
    {
        public string name;
        public string url;
        public string img;
    }

    public class UserLoginItem
    {
        public string userCode;//用户Code
        public string avatar;//头像
        public string name;
        public string token;//token
        public string power;//权限
        public string authority;//前台权限
        public string shopId;//店铺信息
        public bool isonload;//是否登陆成功
        public string msg;//登录失败原因
    }

    public class OrderListItem
    {
        public int key;//序号
        public string img;//
        public string state;//状态
        public string orderCode;//订单码
        public string num;//数量
        public string price;//价格
        public string payTime;//支付时间
    }

    public class OrderListItemList
    {
        public double totalPrice;//总钱数               
    }

    public class OrderDetailsItem
    {
        public int key;
        public string goodsId;
        public string barcode;
        public string goodsName;
        public string num;
        public string supplyPrice;
        public string salePrice;
        public string state;
    }

    public class MemberListItem
    {
        public int key;
        public string name;
        public string phone;
        public string img;
        public string sex;
    }

    public class GoodsListItem
    {
        public int key;
        public string goodsName;
        public string goodsId;
        public string img;
        public string goodsCost;//进货价
        public string goodsPrice;//售价
        public string goodsNum;//库存
        public string goodsNums;//商品数量
        public int ifchose;//0未选中，1选中
    }

    public class ActiveListItem
    {
        public int key;
        public string title;//标题
        public string activeId;
        public string consumeNum;//消费
        public string drainage;//引流
        public string newUser;//拓客
        public string time;//时间
        public string activeType;//活动状态
        public List<string> img = new List<string>();
    }

    public class EmployeeLogonItem
    {
        public int key;
        public string img;//图片
        public string userName;//用户名
        public string sex;//性别
        public string phone;//电话
    }

    public class StockStatisticsItem
    {
        public string movinGoods;//未到商品
        public string arriveGoods;//已到商品
        public string allGoods;//所有商品              
    }

    public class StockStatisticsList
    {
        public int key;//序号 
        public string goodsName;//商品名
        public string goodsImg;//商品图片
        public string goodsnum;//商品数
        public string proportion;//商品占比
    }
    #endregion 

}
