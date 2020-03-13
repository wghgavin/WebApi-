
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading;
using MyWebApiProject.DbToJsonFileTool.DB;
using MyWebApiProject.DbToJsonFileTool.Helper;
using MyWebApiProject.DbToJsonFileTool.Service;
using MyWebApiProject.Model;
namespace MyWebApiProject.DbToJsonFileTool
{
    class Program
    {       
        static void Main(string[] args)
        {
            ProgramMode mode = ConfigHelper.GetProGramType();
            IDbExcute dbExcute = DbExcuteFactory.CreateDbExcute(mode);
            dbExcute.ExcuteDbMethod();
        }
      
    }
}
