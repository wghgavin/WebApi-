using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using MyWebApiProject.Common.DB;
using MyWebApiProject.IService;
//using MyWebApiProject.Service;
using Autofac.Extensions.DependencyInjection;
using MyWebApiProject.Common.Util;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyWebApiProject.AOP;
using MyWebApiProject.Common.LogHelper;

namespace MyWebApiProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
        public IWebHostEnvironment Env { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            BaseDbConfig.ConnectionString = Configuration.GetSection("AppSettings:MySqlConnectionString").Value;
            services.AddControllers();
            services.AddSingleton(new Appsettings(Env.ContentRootPath));
            services.AddSingleton(new LogLock(Env.ContentRootPath));
            //services.AddSingleton<IUserService, UserService>();
            //services.AddSingleton<IOrderInfoService, OrderInfoService>();
            #region JWT 认证
            #region 代码简洁版
            services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddCustomAuth(o => { })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["Audience:Issuer"],
                    ValidAudience = Configuration["Audience:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Audience:Secret"])),
                      // 默认允许 300s  的时间偏移量，设置为0
                      ClockSkew = TimeSpan.Zero
                };
            }); 
            #endregion
            #region 代码复杂版
            //#region 参数
            ////读取配置文件
            //var audienceConfig = Configuration.GetSection("Audience");
            //var symmetricKeyAsBase64 = audienceConfig["Secret"];
            //var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            //var signingKey = new SymmetricSecurityKey(keyByteArray);
            //var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            //#endregion
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})// 也可以直接写字符串，AddAuthentication("Bearer")
            // .AddJwtBearer(o => {
            //     o.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateIssuerSigningKey = true,
            //         IssuerSigningKey = signingKey,
            //         ValidateIssuer = true,
            //         ValidIssuer = audienceConfig["Issuer"],//发行人
            //         ValidateAudience = true,
            //         ValidAudience = audienceConfig["Audience"],//订阅人
            //         ValidateLifetime = true,
            //         ClockSkew = TimeSpan.Zero,
            //         RequireExpirationTime = true,
            //     };
            // }); 
            #endregion
            #endregion
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {

                    Title = "My API",
                    Version = "v1"
                }
              );
                //为 Swagger JSON and UI设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                #region Token绑定到ConfigureServices
                //添加header验证信息
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                // 很重要！这里配置安全校验，和之前的版本不一样
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                // 开启 oauth2 安全描述，必须是 oauth2 这个单词
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion
            });
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //开启认证
            app.UseAuthentication();
            //授权中间件
            app.UseAuthorization();


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default",
                          "{controller=Home}/{action=Index}/{id?}");
            });
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.RoutePrefix = "";
            });
            #endregion
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            #region  Service.dll和Repository.dll注入
            var servicesDllFile = Path.Combine(basePath, "MyWebApiProject.Service.dll");
            var repositoryDllFile = Path.Combine(basePath, "MyWebApiProject.Repository.dll");
            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                throw new Exception("Repository.dll和service.dll 丢失，因为项目解耦了，需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。");
            }

            // AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
            var cacheType = new List<Type>();
            if(Appsettings.app(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<MyApiLogAOP>();
                cacheType.Add(typeof(MyApiLogAOP));
            }
            //获取 Service.dll 程序集服务，并注册
            var assemblyServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblyServices)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy
                .InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。
            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                .AsImplementedInterfaces()
                .InstancePerDependency();
            //.InstancePerDependency();
            // .EnableInterfaceInterceptors();
            #endregion
        }
    }
}
