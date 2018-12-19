using Castle.DynamicProxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using SanFu.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;

namespace SanFu.Commons
{
    public class AopInterceptor : IInterceptor
    {
        private LogModel logmodel = new LogModel();

        public void Intercept(IInvocation invocation)
        {
            logmodel.Id = Guid.NewGuid().ToString();
            logmodel.Namespace = invocation.TargetType.Namespace;
            logmodel.ClassName = invocation.TargetType.Name;
            logmodel.MethodName = invocation.Method.Name;

            var jsonSett = new JsonSerializerSettings
            {
                //Error = delegate (object sender, ErrorEventArgs args)
                //{
                //    //errors.Add(args.ErrorContext.Error.Message);
                //    args.ErrorContext.Handled = false;
 
                //},
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                ReferenceLoopHandling= ReferenceLoopHandling.Ignore,  
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting= Formatting.Indented
            };
            try
            {
                //invocation.Arguments.Select(a => a.GetType());
                if (invocation.Arguments.Any() && invocation.Arguments.Select(a => a.GetType().FullName.Contains("System.Linq.Expressions")).Any())
                {
                    logmodel.Parameter = string.Join("|", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray());
                }else
                {
                    logmodel.Parameter = JsonConvert.SerializeObject(invocation.Arguments, jsonSett);
                }
            }
            catch
            {
            }

            logmodel.LogType = "1";
            logmodel.Ip = "";
            logmodel.Source = "web";

            Stopwatch sw = new Stopwatch();
            sw.Start();
            invocation.Proceed();
            sw.Stop();
            TimeSpan tspan = sw.Elapsed;

            try
            {
                logmodel.ResultValue = JsonConvert.SerializeObject(invocation.ReturnValue, jsonSett);
            }
            catch (Exception)
            {
                logmodel.ResultValue = invocation.ReturnValue.ToString();
            }

            logmodel.InputTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            logmodel.ExecTime = tspan.Milliseconds.ToString();
            try
            {
                Log4NetProvider.Info(invocation.TargetType, JsonConvert.SerializeObject(logmodel, jsonSett));
            }
            catch (Exception)
            {
            }
            
        }
    }
}
