using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using log4net;
using System.Xml;
using System.IO;
using System.Reflection;

namespace MyWebApiProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("Log4net.config"));
            //var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(),typeof)
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
            UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    //webBuilder.ConfigureKestrel(options => options.ListenAnyIP(5000));//用于局域网
                    .UseUrls("http://0.0.0.0:5000");//用于局域网和https://*:5000效果一样
                });
    }
}
