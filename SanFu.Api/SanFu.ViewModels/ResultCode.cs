using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SanFu.ViewModels
{
    public enum ResultCode
    {
        /// <summary>
        /// 默认情况
        /// </summary>
        [Description("")]
        Default = 0,

        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Access = 1,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = -1,

        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error = 1000,

        /// <summary>
        /// 签名失败
        /// </summary>
        [Description("签名错误")]
        SignValidateFail = 1001,

        /// <summary>
        /// 签名失败
        /// </summary>
        [Description("参数格式错误")]
        ParamsValidateFail = 1002,
    }
}
