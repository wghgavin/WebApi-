using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using MyWebApiProject.Common.Hubs;
using MyWebApiProject.Common.LogHelper;
using MyWebApiProject.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.Middlewares
{
    public class SignalRSendMildd
    {
        private readonly RequestDelegate _next;
        private readonly IHubContext<ChatHub> _hubContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="hubContext"></param>
        public SignalRSendMildd(RequestDelegate next, IHubContext<ChatHub> hubContext)
        {
            _next = next;
            _hubContext = hubContext;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (Appsettings.app("Middleware", "SignalR", "Enabled").ObjectToBool())
            {
                await _hubContext.Clients.All.SendAsync("ReceiveUpdate", LogLock.GetLogData());
            }
            await _next(context);
        }
    } 
}
