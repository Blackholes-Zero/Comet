using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Framework.Snowflake
{
    public class InvalidSystemClock : Exception
    {
        public InvalidSystemClock(string message) : base(message)
        {
        }
    }
}