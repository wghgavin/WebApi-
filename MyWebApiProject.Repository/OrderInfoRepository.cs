using System;
using System.Collections.Generic;
using System.Text;
using MyWebApiProject.IRepository;
using MyWebApiProject.IRepository.UnitOfWork;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Repository.Base;
namespace MyWebApiProject.Repository
{
   public class OrderInfoRepository:BaseRepository<OrderInfoEntity>,IOrderInfoRepository
    {
        public OrderInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
