using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanFu.Commons
{
    public class ContainerRepository
    {
        public static ILoggerRepository Log4NetRepository { get; set; }
    }
}
