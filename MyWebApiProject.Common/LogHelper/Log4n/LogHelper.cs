using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Common.LogHelper.Log4n
{
   public class LogHelper
    {
        public static void Info(string txt)
        {
            ILog log = LogManager.GetLogger("NetCoreRepository", "loginfo");
            log.Info(txt);
        }
        public static void Error(string txt)
        {
            ILog log = LogManager.GetLogger("NetCoreRepository", "logerror");
            log.Error(txt);
        }

    }
}
