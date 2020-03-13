using MyWebApiProject.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.Model.Seed
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
        public static async Task SeedDataAsync(MyContext myContext, string WebRootPath)
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
                    Console.WriteLine($"Master DB Type: {MyContext.DbType}");
                    Console.WriteLine($"Master DB ConnectString: {MyContext.ConnectionString}");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("DB Type: " + MyContext.DbType);
                    Console.WriteLine("DB ConnectString: " + MyContext.ConnectionString);
                }
                Console.WriteLine("Create Database...");
                myContext.Db.DbMaintenance.CreateDatabase();
                var classes = Assembly.Load("MyWebApiProject.Model").GetTypes().Where(x => x.Namespace == "MyWebApiProject.Model.DbModel").ToArray();
                myContext.CreateTableByEntity(false, classes);
                Console.WriteLine("Database is  created success!");
                Console.WriteLine();
                if (Appsettings.app(new string[] { "AppSettings", "SeedDBDataEnabled" }).ObjectToBool())
                {
                    Console.WriteLine("Seeding database...");
                    foreach (var item in classes)
                    {
                        if (!(((ReflectionUtil.ExportByClassType(typeof(MyContext), item, "ExitList", null)) as Task<bool>).Result))
                        {
                            string json = FileUtil.ReadFile(string.Format(SeedDataFolder, item.Name));
                            if (json != string.Empty)
                            {
                                object obj = JsonUtil.ParseObjByJson(ReflectionUtil.CreateGeneric(typeof(List<>), item).GetType(), json);
                                bool result = (ReflectionUtil.ExportByClassType(typeof(MyContext), item, "InsertTables", new object[] { obj }) as Task<bool>).Result;
                                if (result) Console.WriteLine($"Tables:{item.Name} Insert Data Suceess!");
                                else Console.WriteLine($"Tables:{item.Name} Insert Data Fail!");
                            }
                        }
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
