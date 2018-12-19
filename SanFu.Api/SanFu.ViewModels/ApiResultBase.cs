using NetCore.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanFu.ViewModels
{
    public class ApiResultBase
    {
        public static ApiResultBase GetInstance(ResultCode code = ResultCode.Default, dynamic result = null, string message = "", string responseTime = "")
        {
            var apiResult = new ApiResultBase();
            apiResult.Code = code;
            apiResult.Message = string.IsNullOrEmpty(message) ? code.GetDescription() : message;
            apiResult.Result = result ?? "";
            apiResult.ResponseTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return apiResult;
        }

        public virtual ResultCode Code { get; set; }

        public virtual string Message { get; set; }

        public virtual dynamic Result { get; set; }

        public virtual string ResponseTime { get; set; }
    }
}
