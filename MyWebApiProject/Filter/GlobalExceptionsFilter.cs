using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using MyWebApiProject.Log4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using StackExchange.Profiling;

namespace MyWebApiProject.Filter
{
    /// <summary>
    /// 全局捕获异常
    /// </summary>
    public class GlobalExceptionsFilter: IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogHelper _loggerHelper;
        public GlobalExceptionsFilter(IWebHostEnvironment env, ILogHelper loggerHelper)
        {
            _env = env;
            _loggerHelper = loggerHelper;
        }
        public void OnException(ExceptionContext context)
        {
            JsonErrorResponse json = new JsonErrorResponse();
            json.Message = context.Exception.Message;//错误信息
            if (_env.IsDevelopment())
            {
                json.DevelopmentMessage = context.Exception.StackTrace;//堆栈信息
            }
            context.Result = new InternalServerErrorObjectResult(json);
            MiniProfiler.Current.CustomTiming("Errors：", json.Message);
            //采用log4net 进行错误日志记录
            _loggerHelper.Error(typeof(GlobalExceptionsFilter), WriteLog(json.Message, context.Exception)); 
            
        }
        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public string WriteLog(string throwMsg, Exception ex)
        {
            return string.Format("\r\n【自定义错误】：{0} \r\n【异常类型】：{1} \r\n【异常信息】：{2} \r\n【堆栈调用】：{3}", 
                new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
        }
    }
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
    //返回错误信息
    public class JsonErrorResponse
    {
        /// <summary>
        /// 生产环境的消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 开发环境的消息
        /// </summary>
        public string DevelopmentMessage { get; set; }
    }
}
