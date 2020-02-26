using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MyWebApiProject.Extensions
{
    public static class MiniProfilerSetup
    {
        public static void AddMiniProfilerSetUp(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddMiniProfiler(optins=> {
                optins.RouteBasePath = "/profilter";
                optins.PopupRenderPosition = StackExchange.Profiling.RenderPosition.Left;
                optins.PopupShowTimeWithChildren = true;

            });
        }
    }
}
