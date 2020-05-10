using MyWebApiProject.IRepository;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Service.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.Service
{
   public class ShopInfoService : BaseService<ShopInfoEntity>, IShopInfoService
    {
        IShopInfoRepository _dal;
        public ShopInfoService(IShopInfoRepository dal)
        {
            this._dal = dal;
        }

        public async Task<List<ShopInfoEntity>> GetShopInfoByAreaId(int areaId)
        {
            return await _dal.Query(obj => obj.AreaId == areaId);
        }

    }
}
