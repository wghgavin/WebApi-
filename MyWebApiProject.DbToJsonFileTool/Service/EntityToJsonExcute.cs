using MyWebApiProject.DbToJsonFileTool.DB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace MyWebApiProject.DbToJsonFileTool.Service
{
    class EntityToJsonExcute : IDbExcute
    {
        private  BlockingCollection<dynamic> _queue_dbTable = new BlockingCollection<dynamic>(new ConcurrentQueue<dynamic>());
        private int classCreateNum = 0;
        private int classTotalNum = 0;
        public void ExcuteDbMethod()
        {
            SqlEntityToJson();
        }
        private void SqlEntityToJson()
        {
            Thread t = new Thread(DealQueueDbTable);
            t.IsBackground = true;
            t.Start();
            DbContext dbContext = new DbContext();
            var classes = Assembly.Load("MyWebApiProject.Model").GetTypes();
            classTotalNum = classes.Length;
            Console.WriteLine("开始执行转化，转化类型是:DbEntiry----JsonFile");
            foreach (var item in classes)
            {
                if (item.Namespace.Equals("MyWebApiProject.Model.DbModel"))
                {
                    string result = dbContext.ExportByClassType(item).Result;
                    _queue_dbTable.Add(new { value = result, name = item.Name });
                }
            }
        }
        private void DealQueueDbTable()
        {
            while (!_queue_dbTable.IsCompleted)
            {
                dynamic obj = _queue_dbTable.Take();
                string dbName = obj.name;
                string jsonValue = obj.value;
                CreateJsonFile(dbName, jsonValue);
                classCreateNum++;
                Console.WriteLine($"数据库实体{dbName}转化成功.........");
                if (classCreateNum == classTotalNum) Console.WriteLine($"共执行{classTotalNum}转化!");
            }
        }
        private void CreateJsonFile(string name, string data)
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
