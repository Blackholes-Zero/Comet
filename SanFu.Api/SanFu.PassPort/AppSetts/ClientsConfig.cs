using System;
using System.Collections.Generic;
using System.Text;

namespace SanFu.PassPort.AppSetts
{
    public class ClientsConfig
    {
        public string Id { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string GrantType { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string Secret { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string RedirectUrl { get; set; }

        public string LogoutRedirectUrl { get; set; }

        public string AllowedCorsOrigins { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string ApiResourceName { get; set; }
    }
}