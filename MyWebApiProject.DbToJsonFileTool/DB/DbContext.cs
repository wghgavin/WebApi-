using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using MyWebApiProject.DbToJsonFileTool.Helper;
using MyWebApiProject.DbToJsonFileTool.Extension;
using System.Threading.Tasks;

namespace MyWebApiProject.DbToJsonFileTool.DB
{
    public class DbContext
    {
        public SqlSugarClient _db;
        public DbContext()
        {
            DbConfig dbConfig = ConfigHelper.GetDbConfig();
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = dbConfig.ConStr,
                DbType = dbConfig.DbType,
                InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了
            });
        }
        /// <summary>
        /// 获取对应表的所有直转为json
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetList<T>()
        {
            return (await _db.Queryable<T>().ToListAsync()).ToJson();
        }
        /// <summary>
        /// 使用type类型作为泛型参数使用方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public  Task<string> ExportByClassType(Type type)
        {
            return  this.GetType().GetMethod("GetList").MakeGenericMethod(type).Invoke(this, null) as Task<string>;
        }

    }
}
