using MyWebApiProject.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyWebApiProject.Common.LogHelper.Log4n;

namespace MyWebApiProject.Extensions
{
    public static class Log4netExtensions
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            if (Appsettings.app("Middleware", "RecordAllLogs", "Enabled").ObjectToBool())
            {
                factory.AddProvider(new Log4NetProvider("Log4net.config"));
            }
            return factory;
        }
    }
}
