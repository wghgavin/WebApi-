using System;
using System.Collections.Generic;
using System.Text;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Service.Base;
using MyWebApiProject.IRepository;
namespace MyWebApiProject.Service
{
   public class UserService:BaseService<UserEntity>,IUserService
    {
        IUserRepository dal;
        public UserService(IUserRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
    }
}
