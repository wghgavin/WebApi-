using MyWebApiProject.Common.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace MyWebApiProject.Common.DB
{
    public class BaseDbConfig
    {
        //public static string ConnectionString { get; set; }
        public static List<DBConnInfo> MutiConnectionString => MutiInitConn();
        public static string CurrentDbId = Appsettings.app(new string[] { "MainDB" });
        private static List<DBConnInfo> MutiInitConn()
        {
            List<DBConnInfo> listDataBase = new List<DBConnInfo>();
            List<DBConnInfo> singleDataBase = new List<DBConnInfo>();
            string path = "appsettings.json";
            using (StreamReader fileReader = new StreamReader(path))
            using (JsonTextReader reader = new JsonTextReader(fileReader))
            {
                JObject jObj = (JObject)JToken.ReadFrom(reader);
                var secDBS = jObj["DBS"];
                if (secDBS != null)
                {
                    for (int i = 0; i < secDBS.Count(); i++)
                    {
                        if (secDBS[i]["Enabled"].ObjectToBool())
                        {
                            listDataBase.Add(new DBConnInfo
                            {
                                ConnId = secDBS[i]["ConnId"].ObjectToString(),
                                ConnStr = secDBS[i]["Connection"].ObjectToString(),
                                DbType = (DataBaseType)secDBS[i]["DBType"].ObjectToInt()
                            }); ;
                        }
                    }
                }
            }
            //单库的情况，只保留一个
            if (!Appsettings.app(new string[] { "MutiDBEnabled" }).ObjectToBool())
            {
                if (listDataBase.Count == 1) return listDataBase;
                else
                {
                    var dbFirst = listDataBase.FirstOrDefault(d => d.ConnId == CurrentDbId);
                    if (dbFirst == null) throw new Exception($"请把appsettings文件内的{CurrentDbId}的Enabled设置为true");
                    dbFirst = listDataBase.FirstOrDefault();
                    singleDataBase.Add(dbFirst);
                    return listDataBase;
                }
            }
            return listDataBase;
        }
    }
    public class DBConnInfo
    {
        public string ConnId { get; set; }
        public string ConnStr { get; set; }
        public DataBaseType DbType { get; set; }
    }
    public enum DataBaseType
    {
        MySql = 0,
        SqlServer = 1,
        Sqlite = 2,
        Oracle = 3,
        PostgreSQL = 4
    }
}
