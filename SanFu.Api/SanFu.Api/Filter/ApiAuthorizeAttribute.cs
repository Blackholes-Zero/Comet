using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanFu.Api.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ApiAuthorizeAttribute:AuthorizeAttribute
    {
        public string Permission { get; set; }

        public ApiAuthorizeAttribute(string permission)
        {
            Permission = permission;
        }
    }
}
