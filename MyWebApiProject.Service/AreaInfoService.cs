using MyWebApiProject.IRepository;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.Service
{
   public class AreaInfoService : BaseService<AreaInfoEntity>, IAreaInfoService
    {
        IAreaInfoRepository _dal;
        public AreaInfoService(IAreaInfoRepository dal)
        {
            this._dal = dal;

        }

        public async Task<AreaInfoEntity> GetAreaInfoByName(string name)
        {
            var result =await _dal.Query(obj => obj.AreaName == name);
            return result.FirstOrDefault();
        }
    }
}
