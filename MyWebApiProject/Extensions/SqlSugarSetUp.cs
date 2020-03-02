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
                return new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = BaseDbConfig.ConnectionString,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true,//默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                    InitKeyType = SqlSugar.InitKeyType.Attribute//默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等
                });

            });
        }
    }
}
