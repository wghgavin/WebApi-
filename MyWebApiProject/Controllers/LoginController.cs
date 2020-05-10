using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiProject.AuthHelper;
using MyWebApiProject.Common.Util;
using MyWebApiProject.IService;
using MyWebApiProject.Model;

namespace MyWebApiProject.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        ISysUserInfoService _sysUserInfoServices;
        IUserRoleService _userRoleServices;
        IRoleService _roleServices;
        PermissionRequirement _requirement;
        IRoleModulePermissionService _roleModulePermissionServices;
        ILoginService loginService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="loginServce"></param>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        /// <param name="requirement"></param>
        /// <param name="roleModulePermissionServices"></param>
        public LoginController(ILoginService loginServce,ISysUserInfoService sysUserInfoServices,IUserRoleService userRoleServices,IRoleService roleServices, PermissionRequirement requirement, IRoleModulePermissionService roleModulePermissionServices)
        {
            this._sysUserInfoServices = sysUserInfoServices;
            this._userRoleServices = userRoleServices;
            this._roleServices = roleServices;
            _requirement = requirement;
            _roleModulePermissionServices = roleModulePermissionServices;
            this.loginService = loginServce;
        }
        [HttpPost]
        public async Task<MessageModel<bool>> Login(string name = "", string pass = "")
        {
            bool result = await loginService.QueryByUserNameAndPwd(name, pass);
            if (result)
            {
                return new MessageModel<bool>
                {
                    msg = "登录成功",
                    success=true
                };
            }
            else
            {
                return new MessageModel<bool>
                {
                    msg = "登录失败，账号或密码错误",
                    success = false
                };
            }
        }

        [HttpGet(Name = "GetJwtToken3")]
        //[Route("GetJwtToken3")]
        public async Task<object> GetJwtToken3(string name = "", string pass = "")
        {
            string jwtStr = string.Empty;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
            {
                return new JsonResult(new
                {
                    Status = false,
                    message = "用户名或密码不能为空"
                });
            }
            pass = Md5Helper.MD5Encrypt32(pass);
            var user = await _sysUserInfoServices.Query(d => d.UloginName == name && d.UloginPWD == pass);
            if (user.Count > 0)
            {
                var userRoles = await _sysUserInfoServices.GetUserRoleNameStr(name, pass);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,name),
                    new Claim(JwtRegisteredClaimNames.Jti,user.FirstOrDefault().UiD.ToString()),
                    new Claim(ClaimTypes.Expiration,(DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds)).ToString())
                };
                claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

                var data = await _roleModulePermissionServices.RoleModuleMaps();
                var list = (from item in data
                            where item.IsDeleted == false
                            orderby item.Id
                            select new PermissionItem
                            {
                                Url = item.Module?.LinkUrl,
                                Role = item.Role?.Name
                            }).ToList();
                _requirement.Permissions = list;
                //用户标识
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaims(claims);
                var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                return new JsonResult(token);
            }
            else
            {
                return new JsonResult(new
                {
                    succss = false,
                    message = "认证失败"
                });
            }
        }
    }
}
