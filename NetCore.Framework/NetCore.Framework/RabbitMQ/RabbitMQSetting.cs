﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Framework.RabbitMQ
{
    public class RabbitMQSetting
    {
        public bool IsEnabled { get; set; }
        public string HostName { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
