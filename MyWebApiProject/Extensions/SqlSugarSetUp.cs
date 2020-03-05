using Microsoft.Extensions.DependencyInjection;
using MyWebApiProject.Common.DB;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.Extensions
{
    /// <summary>
    /// SqlSugar启动服务
    /// </summary>
    public static class SqlSugarSetUp
    {
        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            // 默认添加主数据库连接
            //MainDb.CurrentDbConnId = Appsettings.app(new string[] { "MainDB" });
            services.AddScoped<ISqlSugarClient>(o =>
            {
                var listConfig = new List<ConnectionConfig>();
                BaseDbConfig.MutiConnectionString.ForEach(m => {
                    listConfig.Add(new ConnectionConfig { 
                       ConfigId =m.ConnId,
                       ConnectionString=m.ConnStr,
                       DbType = (DbType)m.DbType,
                       IsAutoCloseConnection = true,//自动关闭，不用close了,
                       IsShardSameThread = false,
                       InitKeyType = InitKeyType.Attribute,
                       AopEvents = new AopEvents
                       {
                           OnLogExecuted = (sql, p) => { 
                           //多库操作此处暂时无效果,在另一个地方
                           }
                       },
                       MoreSettings = new ConnMoreSettings { IsAutoRemoveDataCache = true},
                       
                    });
                });

                return new SqlSugarClient(listConfig);
            });
        }
    }
}
