using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MyWebApiProject.Common.Util
{
    public class JsonUtil
    {
        /// <summary>
        /// JSON格式字符转换为T类型的对象,方法一
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T ParseFormByJson<T>(string jsonStr)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms =
            new MemoryStream(Encoding.UTF8.GetBytes(jsonStr)))
            {
                DataContractJsonSerializer serializer =
                new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }           
        }
        /// <summary>
        /// 直接变为object不作拆箱,方法一
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static object ParseObjByJson(Type classType, string jsonStr)
        {
            object obj = Activator.CreateInstance(classType);
            using(MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr)))
            {
                //var settings = new DataContractJsonSerializerSettings
                //{
                //    DateTimeFormat = new DateTimeFormat("yyyy-MM-dd HH:mm:ss")
                //};
                DataContractJsonSerializer serializer =
                    new DataContractJsonSerializer(classType);
                return serializer.ReadObject(ms);
            }
        }
        /// <summary>
        /// 直接变为object不作拆箱,方法二
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static object ParseObjByJson2(Type classType, string jsonStr)
        {
            var setting = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            return JsonConvert.DeserializeObject(jsonStr, classType,setting);
        }
    }
}
