using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Configuration;
using SqlSugar;
using MyWebApiProject.DbToJsonFileTool.Service;

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
        public static ProgramMode GetProGramType()
        {
            string mode = ConfigurationManager.AppSettings["ProgramMode"];
            ProgramMode mod = (ProgramMode)Enum.Parse(typeof(ProgramMode), mode);
            return mod;
        }

    }

    public class DbConfig
    {
        public string ConStr { get; set; }
        public DbType DbType { get; set; }
    }
}
