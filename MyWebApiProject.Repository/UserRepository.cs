using System;
using System.Collections.Generic;
using System.Text;
using MyWebApiProject.IRepository;
using MyWebApiProject.IRepository.UnitOfWork;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Repository.Base;
namespace MyWebApiProject.Repository
{
   public class UserRepository:BaseRepository<UserEntity>,IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
