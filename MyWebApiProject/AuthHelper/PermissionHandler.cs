using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MyWebApiProject.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyWebApiProject.AuthHelper
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        /// <summary>
        /// services 层注入
        /// </summary>
        public IRoleModulePermissionService _roleModulePermissionServices { get; set; }
        public PermissionHandler(IAuthenticationSchemeProvider schemes, IRoleModulePermissionService roleModulePermissionServices)
        {
            Schemes = schemes;
            _roleModulePermissionServices = roleModulePermissionServices;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var data = await _roleModulePermissionServices.GetRoleModule();
            var list = (from item in data
                        where item.IsDeleted == false
                        orderby item.Id
                        select new PermissionItem
                        {
                            Url = item.Module?.LinkUrl,
                            Role = item.Role?.Name
                        }).ToList();
            requirement.Permissions = list;
            //从AuthorizationHandlerContext转成HttpContext，以便取出表头信息
            var httpContext = (context.Resource as
                AuthorizationFilterContext).HttpContext;
            //请求url
            var questUrl = httpContext.Request.Path.Value.ToLower();
            //判断请求是否停止
            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach(var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name) as IAuthenticationRequestHandler;
                if(handlers !=null&&await handler.HandleRequestAsync())
                {
                    context.Fail();
                    return;
                }
            }
            //判断请求是否拥有凭据,即有没有登录
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                if (result?.Principal != null)
                {
                    httpContext.User = result.Principal;
                    if (requirement.Permissions.GroupBy(g => g.Url).Where(w => w.Key?.ToLower() == questUrl).Count() > 0)
                    {
                        //获取当前用户的角色信息
                        var currentUserRoles = (from item in httpContext.User.Claims
                                                where item.Type == requirement.ClaimType
                                                select item.Value).ToList();
                        //验证权限
                        if(currentUserRoles.Count<0||requirement.Permissions.Where(w=>
         currentUserRoles.Contains(w.Role) && w.Url.ToLower() == questUrl).Count() < 0)
                        {
                            context.Fail();
                            return;
                            //可以在这里设置跳转页面
                            //
                            httpContext.Response.Redirect(requirement.DeniedAction);
                        }
                    }
                    else
                    {
                        context.Fail();
                        return;
                    }
                    //判断过期时间
                    if((httpContext.User.Claims.SingleOrDefault(s=>s.Type==ClaimTypes.Expiration)?.Value)!=null
                        && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) > DateTime.Now)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                        return;
                    }
                    return;
                }
            }
            //判断没有登录时，是否访问登录的url,并且是post氢气，并且是form表单提交，否则失败
            if(questUrl.Equals(requirement.LoginPath.ToLower(),StringComparison.Ordinal)&&
                httpContext.Request.Method.Equals("POST") && httpContext.Request.HasFormContentType)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
                return;
            }
        }
    }
}
