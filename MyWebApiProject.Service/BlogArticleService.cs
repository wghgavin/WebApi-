using System;
using System.Collections.Generic;
using System.Text;
using MyWebApiProject.IService;
using MyWebApiProject.Service.Base;
using MyWebApiProject.IRepository;
using System.Threading.Tasks;
using MyWebApiProject.Model.ViewModel;
using System.Linq;
using MyWebApiProject.Common;
using AutoMapper;
using MyWebApiProject.Model.DbModel;

namespace MyWebApiProject.Service
{
   public class BlogArticleService: BaseService<BlogArticle>, IBlogArticleService
    {
        IBlogArticleRepository _dal;
        IMapper _mapper;
        public BlogArticleService(IBlogArticleRepository dal,IMapper mapper)
        {
            this._dal = dal;
            this.baseDal = dal;
            this._mapper = mapper;
        }

        /// <summary>
        /// 获取视图博客详情信息(一般版本)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BlogViewModels> GetBlogDetails(int id)
        {
            // 此处想获取上一条下一条数据，因此将全部数据list出来，有好的想法请提出
            var bloglist = await base.Query(a => a.bID > 0, a => a.bID);
            var blogArticle = (await _dal.Query(a => a.bID == id)).FirstOrDefault();
            BlogViewModels models = null;
            if (blogArticle != null)
            {
                BlogArticle prevblog;
                BlogArticle nextblog;
                int blogIndex = bloglist.FindIndex(item => item.bID == id);
                if (blogIndex >= 0)
                {
                    try
                    {
                        //上一篇
                        prevblog = blogIndex > 0 ? bloglist[blogIndex - 1] : null;
                        //下一篇
                        nextblog = blogIndex + 1 < bloglist.Count() ? bloglist[blogIndex + 1] : null;
                        models = _mapper.Map<BlogViewModels>(blogArticle);
                        if (nextblog != null)
                        {
                            models.next = nextblog.btitle;
                            models.nextID = nextblog.bID;
                        }
                        if (prevblog != null)
                        {
                            models.previous = prevblog.btitle;
                            models.previousID = prevblog.bID;
                        }                       
                    }
                    catch (Exception) { }
                }
                blogArticle.btraffic += 1;
                await _dal.Update(blogArticle, new List<string> { "btraffic" });
            }
            return models;
        }
        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Caching(AbsoluteExpiration = 10)]
        public async  Task<List<BlogArticle>> GetBlogs()
        {
            var bloglist = await base.Query(a => a.bID > 0, a => a.bID);
            return bloglist;
        }
    }
}
