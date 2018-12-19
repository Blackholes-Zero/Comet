using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanFu.Api.Filter
{
    public class HttpHeaderOperation : IOperationFilter
    {
        public HttpHeaderOperation()
        {
        }

        public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            var actionAttrs = context.ApiDescription.ActionAttributes();

            var isAuthorized = actionAttrs.Any(p => typeof(AuthorizeAttribute).IsAssignableFrom(p.GetType()));

            if (isAuthorized == false) //提供action都没有权限特性标记，检查控制器有没有
            {
                var controllerAttrs = context.ApiDescription.ControllerAttributes();

                isAuthorized = controllerAttrs.Any(p => typeof(AuthorizeAttribute).IsAssignableFrom(p.GetType()));
            }

            var isAllowAnonymous = actionAttrs.Any(a => a.GetType() == typeof(AllowAnonymousAttribute));

            if (isAuthorized && isAllowAnonymous == false)
            {
                operation.Parameters.Add(new NonBodyParameter()
                {
                    Name = "Authorization",  //添加Authorization头部参数
                    In = "header",
                    Type = "string",
                    Required = false
                });
            }

            if (operation.Security == null)
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
            var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
                                        {
                                              {"oauth2", new List<string> { "openid", "profile", "Api" }}
                                        };
            operation.Security.Add(oAuthRequirements);

            //上传文件
            if (!context.ApiDescription.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
            !context.ApiDescription.HttpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            var fileParameters = context.ApiDescription.ActionDescriptor.Parameters.Where(n => n.ParameterType == typeof(IFormFile)).ToList();
            if (fileParameters.Count < 0)
            {
                //operation.Parameters.Clear();
                return;
            }
            operation.Consumes.Add("multipart/form-data");
            foreach (var fileParameter in fileParameters)
            {
                var parameter = operation.Parameters.Single(n => n.Name == fileParameter.Name);
                operation.Parameters.Remove(parameter);
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = parameter.Name,
                    In = "formData",
                    Description = parameter.Description,
                    Required = parameter.Required,
                    Type = "file"
                });
            }
        }
    }
}