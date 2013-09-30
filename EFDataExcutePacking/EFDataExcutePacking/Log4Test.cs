using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace EFDataExcutePacking
{
    class Log4Test
    {
        private static readonly ILog logger = LogManager.GetLogger("Log4NetTest.LogTest");
        public static void WriteLog()
        {
            logger.Debug("bbbbb");
            logger.Debug("oooooo");
        }
    }
}
