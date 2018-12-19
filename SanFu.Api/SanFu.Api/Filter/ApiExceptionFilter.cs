using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SanFu.Commons;
using SanFu.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SanFu.Api.Filter
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public readonly IHostingEnvironment _env;
        private LogModel logmodel = new LogModel();

        public ApiExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            var jsonSett = new JsonSerializerSettings
            {
                Error = delegate (object sender, ErrorEventArgs args)
                {
                    //errors.Add(args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                },
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            //日志
            var controller = context.ActionDescriptor;
            logmodel.Id = Guid.NewGuid().ToString();
            logmodel.Namespace = context.ActionDescriptor.DisplayName;
            logmodel.ClassName = context.ActionDescriptor.DisplayName;
            logmodel.MethodName = context.ActionDescriptor.DisplayName;//string.Join("/", controller.RouteData.Values.Values);
            try
            {
                logmodel.Parameter = JsonConvert.SerializeObject(context.ActionDescriptor.Parameters, jsonSett);
            }
            catch (Exception ex)
            {
                logmodel.Parameter = context.ActionDescriptor.Parameters.ToString();
            }

            logmodel.LogType = "1";
            logmodel.Ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
            logmodel.Source = "web";
            try
            {
                logmodel.Exception = context.Exception != null ? JsonConvert.SerializeObject(context.Exception) : "";
            }
            catch (Exception ex)
            {
                logmodel.Exception = context.Exception.ToString();
            }
            logmodel.ResultValue = context.Result != null ? JsonConvert.SerializeObject(context.Result, jsonSett) : "";
            logmodel.InputTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            logmodel.ExecTime = "";

            //添加日志
            //Log4NetProvider.Error(context.Exception.TargetSite.ReflectedType, logmodel);

            //返回错误信息
            var json = new ErrorResponse("web.Exception");
            if (_env.IsDevelopment())
            {
                json.DeveloperMessage = context.Exception;
            }
            context.Result = new ApplicationErrorResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
        }
    }

    public class ApplicationErrorResult : ObjectResult
    {
        public ApplicationErrorResult(object value) : base(value)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }

    public class ErrorResponse
    {
        public ErrorResponse(string msg)
        {
            Message = msg;
        }

        public string Message { get; set; }
        public object DeveloperMessage { get; set; }
    }
}