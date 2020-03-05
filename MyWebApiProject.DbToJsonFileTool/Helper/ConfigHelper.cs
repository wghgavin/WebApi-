using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Configuration;
using SqlSugar;
namespace MyWebApiProject.DbToJsonFileTool.Helper
{
    public class ConfigHelper
    {
        public static DbConfig GetDbConfig()
        {
            string dbTypeStr = ConfigurationManager.AppSettings["DbType"];
            return new DbConfig
            {
                DbType = (DbType)Enum.Parse(typeof(DbType), dbTypeStr),
                ConStr = ConfigurationManager.ConnectionStrings[dbTypeStr].ConnectionString

            };
         }
    }
    public class DbConfig
    {
        public string ConStr { get; set; }
        public DbType DbType { get; set; }
    }
}
