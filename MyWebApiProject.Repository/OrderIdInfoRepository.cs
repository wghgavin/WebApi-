using MyWebApiProject.IRepository;
using MyWebApiProject.IRepository.UnitOfWork;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Repository
{
   public class OrderIdInfoRepository : BaseRepository<OrderIdInfoEntity>, IOrderIdInfoRepository
    {
        public OrderIdInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
