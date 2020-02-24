using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.Middlewares
{
    public static class MiddlewareHelpers
    {
        /// <summary>
        /// SignalR中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSignalRSendMildd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SignalRSendMildd>();
        }
    }
}
