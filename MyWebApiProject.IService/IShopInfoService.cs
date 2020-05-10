using MyWebApiProject.IService.Base;
using MyWebApiProject.Model.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.IService
{
   public interface IShopInfoService : IBaseService<ShopInfoEntity>
    {
        Task<List<ShopInfoEntity>> GetShopInfoByAreaId(int areaId);
    }
}
