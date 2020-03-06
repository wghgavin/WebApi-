using MyWebApiProject.IService.Base;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.IService
{
   public interface IBlogArticleService:IBaseService<BlogArticle>
    {
        Task<List<BlogArticle>> GetBlogs();
        Task<BlogViewModels> GetBlogDetails(int id);
    }
}
