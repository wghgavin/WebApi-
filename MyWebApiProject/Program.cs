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
using log4net;

namespace MyWebApiProject
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        public static void Main(string[] args)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("Log4net.config"));

            var repo = log4net.LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

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
                    //throw new Exception($"Error occured seeding the Database.\n{e.Message}");
                    log.Error($"Error occured seeding the Database.\n{e.Message}");
                }
                log.Info("123456");
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
            UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.
                    UseStartup<Startup>()
                    //.ConfigureKestrel(options => options.ListenAnyIP(5000));//用于局域网
                    .UseUrls("http://0.0.0.0:5000")
                    .ConfigureLogging((hostingContext, builder) =>
                     {
                         builder.ClearProviders();
                         builder.SetMinimumLevel(LogLevel.Trace);
                         builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                         builder.AddConsole();
                         builder.AddDebug();
                     });//用于局域网和https://*:5000效果一样
                });
    }
}
