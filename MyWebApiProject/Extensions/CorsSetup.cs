using Microsoft.Extensions.DependencyInjection;
using MyWebApiProject.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.Extensions
{
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddCors(c =>
            {
                //↓↓↓↓↓↓↓注意正式环境不要使用这种全开放的处理↓↓↓↓↓↓↓↓↓↓
                //c.AddPolicy("AllRequests", policy =>
                //{
                //    policy
                //    .AllowAnyOrigin()//允许任何源
                //    .AllowAnyMethod()//允许任何方式
                //    .AllowAnyHeader();//允许任何头
                //    //.AllowCredentials();//允许cookie
                //});
                //一般采用这种方法
                c.AddPolicy("LimitRequests", policy =>
                {
                    policy
                   // .WithOrigins(Appsettings.app(new string[] { "Startup", "Cors", "IPs" }).Split(','))//支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
                    .AllowAnyOrigin()
                    .AllowAnyHeader()//Ensures that the policy allows any header.
                    .AllowAnyMethod();
                });
            });
        }
    }
}
