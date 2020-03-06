using Microsoft.Extensions.DependencyInjection;
using MyWebApiProject.Model.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.Extensions
{
    public static class DbSetUp
    {
        public static void AddDbSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<DbSeed>();
            services.AddScoped<MyContext>();
        }
    }
}
