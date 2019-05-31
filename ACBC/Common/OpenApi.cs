using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Common
{
    /// <summary>
    /// API类型分组
    /// </summary>
    public enum ApiType
    {
        OpenApi,
        UserApi,
        GoodsApi,
        OrderApi,
        EmployeeApi,
        MemberApi,
        ActiveApi,
    }

    public enum CheckType
    {
        Open,
        Token,
        OpenToken,
        Sign,
    }

    public enum InputType
    {
        Header,
        Body,
    }

    public abstract class BaseApi
    {
        public string appId;
        public string lang;
        public string code;
        public string method;
        public string token;
        public string sign;
        public string nonceStr;
        public object param;

        public abstract CheckType GetCheckType();
        public abstract ApiType GetApiType();
        public abstract InputType GetInputType();

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{2}; method:{0}; param:{1}", method, param, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            string rets = builder.ToString();
            return rets;
        }
    }

    /// <summary>
    /// 完全开放
    /// </summary>
    public class OpenApi : BaseApi
    {
        public override CheckType GetCheckType()
        {
            return CheckType.Open;
        }

        public override InputType GetInputType()
        {
            return InputType.Body;
        }

        public override ApiType GetApiType()
        {
            return ApiType.OpenApi;
        }

    }

    /// <summary>
    /// 完全开放
    /// </summary>
    public class UserApi : BaseApi
    {
        public override CheckType GetCheckType()
        {
            return CheckType.Open;
        }

        public override InputType GetInputType()
        {
            return InputType.Body;
        }

        public override ApiType GetApiType()
        {
            return ApiType.UserApi;
        }

    }

    /// <summary>
    /// 验证token
    /// </summary>
    public class GoodsApi : BaseApi
    {
        public override CheckType GetCheckType()
        {
            return CheckType.Token;
        }

        public override InputType GetInputType()
        {
            return InputType.Body;
        }

        public override ApiType GetApiType()
        {
            return ApiType.GoodsApi;
        }

    }

    /// <summary>
    /// 验证token
    /// </summary>
    public class OrderApi : BaseApi
    {
        public override CheckType GetCheckType()
        {
            return CheckType.Token;
        }

        public override InputType GetInputType()
        {
            return InputType.Body;
        }

        public override ApiType GetApiType()
        {
            return ApiType.OrderApi;
        }

    }

    /// <summary>
    /// 验证token
    /// </summary>
    public class EmployeeApi : BaseApi
    {
        public override CheckType GetCheckType()
        {
            return CheckType.Token;
        }

        public override InputType GetInputType()
        {
            return InputType.Body;
        }

        public override ApiType GetApiType()
        {
            return ApiType.EmployeeApi;
        }
    }

    /// <summary>
    /// 验证token
    /// </summary>
    public class MemberApi : BaseApi
    {
        public override CheckType GetCheckType()
        {
            return CheckType.Token;
        }

        public override InputType GetInputType()
        {
            return InputType.Body;
        }

        public override ApiType GetApiType()
        {
            return ApiType.MemberApi;
        }
    }
    

    /// <summary>
    /// 验证token
    /// </summary>
    public class ActiveApi : BaseApi
    {
        public override CheckType GetCheckType()
        {
            return CheckType.Token;
        }

        public override InputType GetInputType()
        {
            return InputType.Body;
        }

        public override ApiType GetApiType()
        {
            return ApiType.ActiveApi;
        }
    }
}
