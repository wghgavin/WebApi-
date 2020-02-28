using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiProject.AuthHelper;
using MyWebApiProject.IService;
using MyWebApiProject.Model;
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
        [HttpGet]
        //[Route("jsonp")]
        public void Getjsonp(string callBack, long id = 1, string sub = "Admin", int expiresSliding = 30, int expiresAbsoulute = 30)
        {
            TokenModelJwt tokenModel = new TokenModelJwt();
            tokenModel.Uid = id;
            tokenModel.Role = sub;           
            DateTime d1 = DateTime.Now;
            DateTime d2 = d1.AddMinutes(expiresSliding);
            DateTime d3 = d1.AddDays(expiresAbsoulute);
            TimeSpan sliding = d2 - d1;
            TimeSpan absoulute = d3 - d1;
            var jwtStr = JwtHelper.IssueJwt(tokenModel);

            string response = string.Format("\"value\":\"{0}\"", jwtStr);
            string call = callBack + "({" + response + "})";
            Response.WriteAsync(call);
        }
        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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
        [HttpGet]
        //[Route("GetJwtStr")]
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
        [HttpGet]
        //[Route("GetAllUserInfo")]
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
        [HttpGet]
        //[Route("GetUserInfoByPwd")]
        public async Task<List<UserEntity>> GetUserInfoByPwd(string pwd)
        {
            return await userService.Query(it => it.PassWord == pwd && it.UserName == "SuperUser");
        }
        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        //[Route("GetUserInfoByUserName")]
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
        [HttpGet]
        //[Route("GetUserByUserNameAndPwd")]
        public async Task<List<UserEntity>> GetUserByUserNameAndPwd(string userName, string pwd)
        {
            return await userService.Query(it => it.UserName == userName && it.PassWord == pwd);
        }
        /// <summary>
        /// 增加用户信息
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        [HttpPost]
        //[Route("AddUserInfo")]
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
        //[Route("DeleteUserInfo")]
        public async Task<bool> DeleteUserInfo(string userName)
        {
            return await userService.DeleteByExpression(it => it.UserName == userName);
        }
    }
}
