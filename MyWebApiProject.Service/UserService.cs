using System;
using System.Collections.Generic;
using System.Text;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Service.Base;
using MyWebApiProject.IRepository;
using System.Threading.Tasks;

namespace MyWebApiProject.Service
{
    public class UserService:BaseService<UserEntity>, IUserService
    {
        IUserRepository dal;
        public UserService(IUserRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       public async Task<List<UserEntity>> getBlogs()
        {
            return await dal.Query();
        }
    }
}
