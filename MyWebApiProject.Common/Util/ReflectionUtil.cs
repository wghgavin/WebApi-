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
        /// <summary>
        /// 创建泛型类
        /// </summary>
        /// <param name="generic">泛型</param>
        /// <param name="innerType">类别T的type</param>
        /// <param name="args">构造函数的参数</param>
        /// <returns></returns>
        public static object CreateGeneric(Type generic, Type innerType, params object[] args)
        {
            Type specificType = generic.MakeGenericType(new System.Type[] { innerType });
            return Activator.CreateInstance(specificType, args);
        }
    }
}
