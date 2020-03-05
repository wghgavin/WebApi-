
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading;
using MyWebApiProject.DbToJsonFileTool.DB;
using MyWebApiProject.Model;
namespace MyWebApiProject.DbToJsonFileTool
{
    class Program
    {
        private static BlockingCollection<dynamic> _queue_dbTable = new BlockingCollection<dynamic>(new ConcurrentQueue<dynamic>());
        static int num = 0;
        static void Main(string[] args)
        {
            Thread t = new Thread(DealQueueDbTable);
            t.IsBackground = true;
            t.Start();
            DbContext dbContext = new DbContext();
            var classes = Assembly.Load("MyWebApiProject.Model").GetTypes();
            foreach (var item in classes)
            {
                if (item.Namespace.Equals("MyWebApiProject.Model.DbModel") || item.Namespace.Equals("MyWebApiProject.Model.TestModel"))
                {
                    string result = dbContext.ExportByClassType(item).Result;
                    _queue_dbTable.Add(new { value = result, name = item.Name });
                }
            }
        }
        static void DealQueueDbTable()
        {
            while (!_queue_dbTable.IsCompleted)
            {
                dynamic obj = _queue_dbTable.Take();
                string dbName = obj.name;
                string jsonValue = obj.value;
                CreateJsonFile(dbName, jsonValue);
            }
        }
        static void CreateJsonFile(string name,string data)
        {
            string directionPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DbJson");
            if (!Directory.Exists(directionPath))
            {
                Directory.CreateDirectory(directionPath);
            }
            File.WriteAllText(Path.Combine(directionPath, name + ".tsv"), data);
        }
    }
}
