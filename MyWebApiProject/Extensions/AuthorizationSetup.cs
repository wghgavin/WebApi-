using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyWebApiProject.AuthHelper;
using MyWebApiProject.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.Extensions
{
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            //读取配置
            var symmetricKeyAsBase64 = Appsettings.app("Audience", "Secret");
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            //令牌验证参,之前写在AddJwtBearer里面
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,//验证发行人的签名秘钥
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,//验证发行人
                ValidIssuer = Appsettings.app("Audience", "Issuer"),
                ValidateAudience =true,//验证订阅人
                ValidateLifetime = true,//验证生命周期
                ClockSkew = TimeSpan.Zero,//定义的过期缓存时间
                RequireExpirationTime = true,//是否要求过期
            };
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
           //数据库动态绑定
            var permission = new List<PermissionItem>();
            //角色与接口的权限要求参数
            var permissionRequirenment = new PermissionRequirement(
                "拒绝授权的跳转地址",//(目前无用)
                permission,
                ClaimTypes.Role,//角色授权,
                Appsettings.app("Audience", "Issuer"),//发行人
                Appsettings.app("Audience", "Audience"),//订阅人
                signingCredentials,
                expiration:TimeSpan.FromSeconds(60*2)///接口的过期时间，注意这里没有了缓冲时间，
//你也可以自定义，在上边的TokenValidationParameters的 ClockSkew
                );
            // ① 核心之一，配置授权服务，也就是具体的规则，已经对应的权限策略，比如公司不同权限的门禁卡
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client",
                    policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin",
                    policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("SystemOrAdmin",
                    policy => policy.RequireRole("Admin", "System"));
            })
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o => {
                o.TokenValidationParameters = tokenValidationParameters;
            });
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            //将授权的必要类注入生命周期
            services.AddSingleton(permissionRequirenment);
        }
    }
}
