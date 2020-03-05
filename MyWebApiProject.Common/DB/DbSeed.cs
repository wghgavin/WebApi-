using MyWebApiProject.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.Common.DB
{
   public class DbSeed
    {
        private static string SeedDataFolder = "DbJson/{0}.tsv";
        /// <summary>
        /// 异步添加种子数据
        /// </summary>
        /// <param name="myContext"></param>
        /// <param name="WebRootPath"></param>
        /// <returns></returns>
        public static async Task SeedDataAsync(DbContext myContext, string WebRootPath)
        {
            try
            {
                if (string.IsNullOrEmpty(WebRootPath))
                {
                    throw new Exception("获取wwwroot路径时，异常！");
                }
                SeedDataFolder = Path.Combine(WebRootPath, SeedDataFolder);
                Console.WriteLine("Config data init...");
                Console.WriteLine($"Is multi-DataBase: {Appsettings.app(new string[] { "MutiDBEnabled" })}");
                if (Appsettings.app(new string[] { "MutiDBEnabled" }).ObjectToBool())
                {
                    Console.WriteLine($"Master DB Type: {DbContext.DbType}");
                    Console.WriteLine($"Master DB ConnectString: {DbContext.ConnectionString}");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("DB Type: " + DbContext.DbType);
                    Console.WriteLine("DB ConnectString: " + DbContext.ConnectionString);
                }
                Console.WriteLine("Create Database...");
                myContext.Db.DbMaintenance.CreateDatabase();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
