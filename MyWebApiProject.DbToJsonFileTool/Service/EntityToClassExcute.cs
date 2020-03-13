using MyWebApiProject.DbToJsonFileTool.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MyWebApiProject.DbToJsonFileTool.Service
{
    class EntityToClassExcute : IDbExcute
    {
        public void ExcuteDbMethod()
        {
            DbContext dbContext = new DbContext();
            dbContext.Create_Model_ClassFileByDBTalbe(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassSaveDir"), "MyWebApiProject.Model.DbModel",null);
            Console.WriteLine("实体类生成成功");
         }
       
    }
}
