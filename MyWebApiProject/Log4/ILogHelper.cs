using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.Log4
{
   public interface ILogHelper
    {

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        void Debug(Type source, string message);

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        void Info(Type source, object message);

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        void Warn(Type source, object message);

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        void Error(Type source, object message);

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        void Fatal(Type source, object message);

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        void Debug(Type source, object message, Exception exception);

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        void Info(Type source, object message, Exception exception);
 
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        void Warn(Type source, object message, Exception exception);

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        void Error(Type source, object message, Exception exception);
        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        void Fatal(Type source, object message, Exception exception);
    }
}
