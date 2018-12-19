using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Framework.Cache
{
    public class RedisCacheOptions
    {
        public string Configuration { get; set; }

        public string InstanceName { get; set; }

        public int DefaultDatabase { get; set; }
    }
}
