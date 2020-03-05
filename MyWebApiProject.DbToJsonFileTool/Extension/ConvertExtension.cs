using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.DbToJsonFileTool.Extension
{
   public static class ConvertExtension
    {
        public static string  ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
