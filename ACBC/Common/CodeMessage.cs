using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Common
{
    /// <summary>
    /// 返回信息对照
    /// </summary>
    public enum CodeMessage
    {
        OK = 0,
        PostNull = -1,

        AppIDError = 201,
        SignError = 202,

        ErrorLogin=401,//登录失败
        ErrorLogout = 402,//登出失败
        NotFound = 404,
        InnerError = 500,
        DBAddError =601,//插入数据库错误
        DBDelError=602,//删除数据库错误
        DBUpdateError = 603,//更新数据库错误
        DBSelectError=600,//数据库错误

        InvalidToken = 4000,
        InvalidMethod = 4001,
        InvalidParam = 4002,
        InterfaceRole = 4003,
        InterfaceValueError = 4004,
        InterfaceDBError = 4005,//操作数据库错误
        NeedLogin = 4006,
        InvalidCode = 4007,
        InsufficientAuthority=5001,//权限不足
        CodeError=6000,//验证码错误
        CodeRepeat=6001,//验证码重复
        ErrorLogonNum=6002,//错误注册数量

        InvalidGoodsIdCode=7001,//商品id错误
        InvalidGoodsNumCode = 7002,//商品数量错误

        InvalidActiveType=8001,//活动类型错误
        InvalidConsume=8002,//填写钱数错误
        InvalidTime=8003,//时间错误
        InvalidRemark=8004,//活动标题错误
        InvalidGift=8005,//礼品错误
        InvalidStatus=8006,//状态错误
        ErrorActiveId=8007,//活动单号错误
        ErrorOperation=8008,//活动操作错误
        ErrorState=8009,//活动状态错误
        ErrorHeartItemValue=8010,//心值不是整数
        ErrorLimitItemValue=8011,//上限不是整数

        ErrorOrderCode =9001,//ordercode错误

        ErrorEquipmentId=20001,//分组参数错误

        InvalidEnvAndGroup = 5000,
    }
}
