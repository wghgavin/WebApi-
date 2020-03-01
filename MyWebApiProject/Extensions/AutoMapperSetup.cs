using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MyWebApiProject.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.Extensions
{
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentException(nameof(services));
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }
    }
}
