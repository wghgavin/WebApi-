using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Xml;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MyWebApiProject.Common.Util;
using MyWebApiProject.Model.Seed;

namespace MyWebApiProject
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // 创建可用于解析作用域服务的新 Microsoft.Extensions.DependencyInjection.IServiceScope。
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // 从 system.IServicec提供程序获取 T 类型的服务。
                    var configuration = services.GetRequiredService<IConfiguration>();
                    if (configuration.GetSection("AppSettings")["SeedDBEnabled"].ObjectToBool() || configuration.GetSection("AppSettings")["SeedDBDataEnabled"].ObjectToBool())
                    {
                        var myContext = services.GetRequiredService<MyContext>();
                        var Env = services.GetRequiredService<IWebHostEnvironment>();
                        DbSeed.SeedDataAsync(myContext, Env.WebRootPath).Wait();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"Error occured seeding the Database.\n{e.Message}");
                    throw;
                }
            }
            host.Run();
            //  CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
            UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.
                    UseStartup<Startup>()
                    //webBuilder.ConfigureKestrel(options => options.ListenAnyIP(5000));//用于局域网
                    .UseUrls("http://0.0.0.0:5000");//用于局域网和https://*:5000效果一样
                });
    }
}
