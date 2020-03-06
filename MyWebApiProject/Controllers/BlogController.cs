using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Model.ViewModel;

namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        IBlogArticleService blogArticleService;
        public BlogController(IBlogArticleService blogArticleService)
        {
            this.blogArticleService = blogArticleService;
        }
        /// <summary>
        /// 获取指定id的博客信息
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetBlogDetails")]

        public async Task<BlogViewModels> GetBlogDetails(int id)
        {
            return await blogArticleService.GetBlogDetails(id);
        }
        /// <summary>
        /// 获取所有博客信息
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetBlogs")]
        public async Task<List<BlogArticle>> GetBlogs()
        {
            return await blogArticleService.GetBlogs();
        }
    }
}