using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyWebApiProject.DbToJsonFileTool.Service
{
    public enum ProgramMode
    {
        EntityToJsonExcute,
        EntityToClassExcute
    }
    
    public  class DbExcuteFactory
    {
        public static IDbExcute CreateDbExcute(ProgramMode mode)
        {
            string name = Enum.GetName(typeof(ProgramMode), mode);
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
            IDbExcute dbExcute = (IDbExcute)assembly.CreateInstance($"MyWebApiProject.DbToJsonFileTool.Service.{name}");
            return dbExcute;
        }
    }
}
