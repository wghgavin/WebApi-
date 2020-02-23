using System;
using System.Collections.Generic;
using System.Text;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.IService.Base;
using System.Threading.Tasks;

namespace MyWebApiProject.IService
{
   public interface IUserService:IBaseService<UserEntity>
    {
        Task<List<UserEntity>> getBlogs();
    }
}
