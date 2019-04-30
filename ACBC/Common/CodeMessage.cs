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

        NotFound = 404,
        InnerError = 500,

        InvalidToken = 4000,
        InvalidMethod = 4001,
        InvalidParam = 4002,
        InterfaceRole = 4003,
        InterfaceValueError = 4004,
        InterfaceDBError = 4005,
        NeedLogin = 4006,
        InvalidCode = 4007,

        InvalidEnvAndGroup = 5000,
    }
}
