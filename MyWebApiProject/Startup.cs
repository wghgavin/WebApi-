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
using Autofac.Extensions.DependencyInjection;
using MyWebApiProject.Common.Util;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyWebApiProject.AOP;
using MyWebApiProject.Common.LogHelper;
using Microsoft.AspNetCore.Http;
using MyWebApiProject.Middlewares;
using MyWebApiProject.Common.Hubs;
using MyWebApiProject.Extensions;
using MyWebApiProject.Common.Redis;
using log4net.Repository;
using log4net;
using log4net.Config;
using MyWebApiProject.Log4;
using MyWebApiProject.Filter;

namespace MyWebApiProject
{
    public class Startup
    {
        public static ILoggerRepository repository { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            repository = LogManager.CreateRepository("");//��Ҫ��ȡ��־�Ĳֿ�����Ҳ������ĵ�ǰ��Ŀ��
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));//�����ļ�
            Configuration = configuration;
            Env = env;
        }
        public IWebHostEnvironment Env { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();
            services.AddSingleton(new Appsettings(Env.ContentRootPath));
            services.AddSingleton(new LogLock(Env.ContentRootPath));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMemoryCacheSetup();
            services.AddCorsSetup();
            services.AddMiniProfilerSetUp();
            services.AddSignalR();
            services.AddAutoMapperSetup();
            services.AddSqlsugarSetup();
            services.AddDbSetup();
            services.AddSingleton<ILogHelper, LogHelper>();
            services.AddControllers(o =>
            {
                // ȫ���쳣����
                o.Filters.Add(typeof(GlobalExceptionsFilter));
            });
            #region JWT ��֤
            #region �������
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
                    // Ĭ������ 300s  ��ʱ��ƫ����������Ϊ0
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion
            #region ���븴�Ӱ�
            //#region ����
            ////��ȡ�����ļ�
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
            //})// Ҳ����ֱ��д�ַ�����AddAuthentication("Bearer")
            // .AddJwtBearer(o => {
            //     o.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateIssuerSigningKey = true,
            //         IssuerSigningKey = signingKey,
            //         ValidateIssuer = true,
            //         ValidIssuer = audienceConfig["Issuer"],//������
            //         ValidateAudience = true,
            //         ValidAudience = audienceConfig["Audience"],//������
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
                //Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                var xmlPath_model = Path.Combine(AppContext.BaseDirectory, "MyWebApiProject.Model.xml");
                c.IncludeXmlComments(xmlPath_model, true);
                #region Token�󶨵�ConfigureServices
                //���header��֤��Ϣ
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                // ����Ҫ���������ð�ȫУ�飬��֮ǰ�İ汾��һ��
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                // ���� oauth2 ��ȫ������������ oauth2 �������
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //app.UseSignalRSendMildd();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("LimitRequests");
            //��תhttps
            //app.UseHttpsRedirection();
            // ʹ�þ�̬�ļ�
            app.UseStaticFiles();
            // ʹ��cookie
            app.UseCookiePolicy();
            // ���ش�����
            app.UseStatusCodePages();//�Ѵ����뷵��ǰ̨��������404

            app.UseRouting();
            //������֤
            app.UseAuthentication();
            //��Ȩ�м��
            app.UseAuthorization();

            app.UseMiniProfiler();//miniProfiler���������м��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default",
                          "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/chatHub");
            });
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.RoutePrefix = "";
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("MyWebApiProject.index.html");
            });
            #endregion
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            #region  Service.dll��Repository.dllע�룬AOPע��
            var servicesDllFile = Path.Combine(basePath, "MyWebApiProject.Service.dll");
            var repositoryDllFile = Path.Combine(basePath, "MyWebApiProject.Repository.dll");
            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                throw new Exception("Repository.dll��service.dll ��ʧ����Ϊ��Ŀ�����ˣ���Ҫ��F6���룬��F5���У����� bin �ļ��У���������");
            }
            // AOP ���أ������Ҫ��ָ���Ĺ��ܣ�ֻ��Ҫ�� appsettigns.json ��Ӧ��Ӧ true ���С�
            var cacheType = new List<Type>();
            if (Appsettings.app(new string[] { "AppSettings", "RedisCachingAOP", "Enabled" }).ObjectToBool())
            {
                builder.RegisterType<MyApiRedisCacheAOP>();
                cacheType.Add(typeof(MyApiRedisCacheAOP));
            }
            if (Appsettings.app(new string[] { "AppSettings", "MemoryCachingAOP", "Enabled" }).ObjectToBool())
            {
                builder.RegisterType<MyApiCacheAOP>();
                cacheType.Add(typeof(MyApiCacheAOP));
            }
            if (Appsettings.app(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjectToBool())
            {
                builder.RegisterType<MyApiLogAOP>();
                cacheType.Add(typeof(MyApiLogAOP));
            }
            //��ȡ Service.dll ���򼯷��񣬲�ע��
            var assemblyServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblyServices)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()//����Autofac.Extras.DynamicProxy//
                .InterceptedBy(cacheType.ToArray());//����������������б�����ע�ᡣ
                                                    //��ȡ Repository.dll ���򼯷��񣬲�ע��
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                .AsImplementedInterfaces()
                .InstancePerDependency();
            #endregion
        }
    }
}
