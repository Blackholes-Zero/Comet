using System;
using System.Collections.Generic;
using System.Text;

namespace SanFu.PassPort.AppSetts
{
    public class IdentityServer
    {
        public List<ApiResourcesConfig> ApiResources { get; set; }
        public List<ClientsConfig> Clients { get; set; }
    }
}