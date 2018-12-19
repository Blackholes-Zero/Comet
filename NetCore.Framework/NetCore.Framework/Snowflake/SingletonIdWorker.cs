using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Framework.Snowflake
{
    public class SingletonIdWorker
    {
        private static IdWorker _instance;
        private static readonly object syslock = new object();

        public static IdWorker GetInstance()
        {
            if (_instance == null)
            {
                lock (syslock)
                {
                    if (_instance == null)
                    {
                        _instance = new IdWorker(1, 1);
                    }
                }
            }
            return _instance;
        }
    }
}