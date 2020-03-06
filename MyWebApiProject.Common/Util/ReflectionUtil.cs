using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Common.Util
{
   public  class ReflectionUtil
    {
      
        /// <summary>
        /// 使用type类型作为泛型参数使用方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object? ExportByClassType(Type classType,Type genericType, string excuteMethodStr, object[] param)
        {
            object? obj = classType.GetMethod(excuteMethodStr).MakeGenericMethod(genericType).Invoke(Activator.CreateInstance(classType), param);
            return obj;
        }
    }
}
