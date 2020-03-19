using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using MyWebApiProject.Common.Hubs;
using MyWebApiProject.Common.LogHelper;
using Newtonsoft.Json;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyWebApiProject.AOP
{
    public class MyApiLogAOP: IInterceptor
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IHttpContextAccessor _accessor;

        public MyApiLogAOP(IHubContext<ChatHub> hubContext, IHttpContextAccessor accessor)
        {
            _hubContext = hubContext;
            _accessor = accessor;
        }
        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            //记录被拦截方法信息的日志信息
            var dataIntercept = $"{DateTime.Now.ToString("yyyyMMddHHmmss")} " +
                $"当前执行方法：{ invocation.Method.Name} " +
                $"参数是： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";
            #region 异步
            try
            {
                MiniProfiler.Current.Step($"执行Service方法：{invocation.Method.Name}() -> ");
                //在被拦截的方法执行完毕后 继续执行当前方法，注意是被拦截的是异步的
                invocation.Proceed();
                // 异步获取异常，先执行
                if (IsAsyncMethod(invocation.Method)){
                    //Wait task execution and modify return value
                    if (invocation.Method.ReturnType == typeof(Task))//异步非泛型
                    {
                        invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                            (Task)invocation.ReturnValue,
                            async () => await SuccessAction(invocation, dataIntercept)
                            ,
                             ex =>
                             {
                                 LogEx(ex, dataIntercept);
                             }
                            ) ;
                    }
                    else//异步泛型
                    {
                         invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                         invocation.Method.ReturnType.GenericTypeArguments[0],                         
                         invocation.ReturnValue,
                         async () => await SuccessAction(invocation, dataIntercept),
                         ex =>
                         {
                             LogEx(ex,dataIntercept);
                         });;
                    }
                }
                else
                {// 同步1


                }
            }
            catch (Exception ex)//同步2
            {
                LogEx(ex,dataIntercept);
            }
            #endregion

         
            _hubContext.Clients.All.SendAsync("ReceiveUpdate", LogLock.GetLogData()).Wait();


        }
        private async Task SuccessAction(IInvocation invocation, string dataIntercept)
        {
            var type = invocation.Method.ReturnType;
            if (typeof(Task).IsAssignableFrom(type))
            {
                var resultProperty = type.GetProperty("Result");
                dataIntercept += $"【执行完成结果】：{JsonConvert.SerializeObject(resultProperty.GetValue(invocation.ReturnValue))}";
            }
            else
            {
                dataIntercept += ($"【执行完成结果】：{invocation.ReturnValue}");//同步
            }
            Parallel.For(0, 1, e =>
            {
                LogLock.OutSql2Log("AOPLog", new string[] { dataIntercept });
            });
        }
        public static bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                );
        }
        private void LogEx(Exception ex, string dataIntercept)
        {
            if (ex != null)
            {
                //执行的 service 中，收录异常
                MiniProfiler.Current.CustomTiming("Errors：", ex.Message);
                //执行的 service 中，捕获异常
                dataIntercept += ($"方法执行中出现异常：{ex.Message + ex.InnerException}\r\n");
                Parallel.For(0, 1, e => {
                    LogLock.OutSql2Log("AOPLog", new string[] { dataIntercept });
                });
            }
        }
    }
    internal static class InternalAsyncHelper
    {
        public static async Task AwaitTaskWithPostActionAndFinally(Task actualReturnValue,Func<Task> successAction,Action<Exception> finalAction)
        {
            Exception exception = null;
            try
            {
                await actualReturnValue;
                await successAction();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                finalAction(exception);
            }
        }
        public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetResult<T>(Task<T> actualReturnValue, Func<Task> successAction, Action<Exception> finalAction)
        {
            Exception exception = null;
            try
            {
                var result = await actualReturnValue;
                await successAction();
                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }
        public static object CallAwaitTaskWithPostActionAndFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Func<Task> successAction, Action<Exception> finalAction)
        {
            return typeof(InternalAsyncHelper)
                .GetMethod("AwaitTaskWithPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue, successAction, finalAction });
        }
    }
}
