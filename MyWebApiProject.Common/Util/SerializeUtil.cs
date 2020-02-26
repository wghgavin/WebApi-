using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Common.Util
{
   public class SerializeUtil
    {
        /// <summary>
        /// 序列化（字节数组）
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static byte[] SerializeToByteArray(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);

            return Encoding.UTF8.GetBytes(jsonString);
        }
        /// <summary>
        /// 反序列化（字节数组）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEntity ByteArrayDeserialize<TEntity>(byte[] value)
        {
            if (value == null)
            {
                return default(TEntity);
            }
            var jsonString = Encoding.UTF8.GetString(value);
            return JsonConvert.DeserializeObject<TEntity>(jsonString);
        }
    }
}
