using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.Log4
{
    public class LogHelper:ILogHelper
    {
        private readonly ConcurrentDictionary<Type, ILog> Loggers = new ConcurrentDictionary<Type, ILog>();
        /// <summary>
        /// 获取记录器
        /// </summary>
        /// <param name="source">soruce</param>
        /// <returns></returns>
        private ILog GetLogger(Type source)
        {
            if (Loggers.ContainsKey(source))
            {
                return Loggers[source];
            }
            else
            {
                ILog logger = LogManager.GetLogger(Startup.repository.Name, source);
                Loggers.TryAdd(source, logger);
                return logger;
            }
        }

        #region 错误信息
        public void Error(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsErrorEnabled)
            {
                logger.Error(message);
            }
        }

        public void Error(Type source, object message, Exception exception)
        {
            GetLogger(source).Error(message, exception);
        } 
        #endregion

        #region 调试信息

        public void Debug(Type source, string message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsDebugEnabled)
            {
                logger.Debug(message);
            }
        }

        public void Debug(Type source, object message, Exception exception)
        {
            GetLogger(source).Debug(message, exception);
        }
        #endregion
        #region 记录关键信息

        public void Info(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsInfoEnabled)
            {
                logger.Info(message);
            }
        }

        public void Info(Type source, object message, Exception exception)
        {
            GetLogger(source).Info(message, exception);
        }
        #endregion
        #region 记录警告信息
 
        public void Warn(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsWarnEnabled)
            {
                logger.Warn(message);
            }
        }


        public void Warn(Type source, object message, Exception exception)
        {
            GetLogger(source).Warn(message, exception);
        }
        #endregion
        #region 失败信息

        public void Fatal(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsFatalEnabled)
            {
                logger.Fatal(message);
            }
        }
        public void Fatal(Type source, object message, Exception exception)
        {
            GetLogger(source).Fatal(message, exception);
        }
        #endregion










    }
}
