using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiProject.AuthHelper;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using StackExchange.Profiling;

namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetBlogs")]
        //[Route("GetBlogs")]
        public async Task<List<UserEntity>> GetBlogs()
        {
            return await userService.getBlogs();
        }
        /// <summary>
        /// 获取Token的令牌
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetJwtStr")]
        public async Task<object> GetJwtStr(string name, string pass)
        {
            // 将用户id和角色名，作为单独的自定义变量封装进 token 字符串中。
            TokenModelJwt tokenModel = new TokenModelJwt { Uid = 1, Role = "Admin" };
            var jwtStr = JwtHelper.IssueJwt(tokenModel);//登录，获取到一定规则的 Token 令牌
            var suc = true;
            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }
        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllUserInfo")]
        [Authorize(Roles = "Admin")]
        public async Task<List<UserEntity>> GetAllUserInfo()
        {
            return await userService.Query(it => it.UserName != "SuperUser");
        }
        /// <summary>
        /// 根据密码获取超级用户信息
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetUserInfoByPwd")]
        public async Task<List<UserEntity>> GetUserInfoByPwd(string pwd)
        {
            return await userService.Query(it => it.PassWord == pwd && it.UserName == "SuperUser");
        }
        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetUserInfoByUserName")]
        public async Task<List<UserEntity>> GetUserInfoByUserName(string userName)
        {
            return await userService.Query(it => it.UserName == userName);
        }
        /// <summary>
        /// 根据用户名和密码获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetUserByUserNameAndPwd")]
        public async Task<List<UserEntity>> GetUserByUserNameAndPwd(string userName, string pwd)
        {
            return await userService.Query(it => it.UserName == userName && it.PassWord == pwd);
        }
        /// <summary>
        /// 增加用户信息
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        [HttpPost(Name = "AddUserInfo")]
        public async Task<int> AddUserInfo(UserEntity userEntity)
        {
            return await userService.Add(userEntity);
        }
        /// <summary>
        /// 根据用户名删除
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpDelete("{userName}", Name = "DeleteUserInfo")]
        public async Task<bool> DeleteUserInfo(string userName)
        {
            return await userService.DeleteByExpression(it => it.UserName == userName);
        }
    }
}
