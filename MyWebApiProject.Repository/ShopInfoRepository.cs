using MyWebApiProject.IRepository;
using MyWebApiProject.IRepository.UnitOfWork;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Repository
{
   public class ShopInfoRepository : BaseRepository<ShopInfoEntity>, IShopInfoRepository
    {
        public ShopInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
