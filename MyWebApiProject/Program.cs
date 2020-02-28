using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyWebApiProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
            UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    //webBuilder.ConfigureKestrel(options => options.ListenAnyIP(5000));//���ھ�����
                    .UseUrls("http://*:5000");//���ھ�����
                    //.ConfigureLogging((hostingContext, builder) =>
                    //  {
                    //      builder.ClearProviders();
                    //      builder.SetMinimumLevel(LogLevel.Trace);
                    //      builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    //      builder.AddConsole();
                    //      builder.AddDebug();
                    //  });
                });
    }
}
