using MyWebApiProject.IService.Base;
using MyWebApiProject.Model.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.IService
{
   public interface IAreaInfoService : IBaseService<AreaInfoEntity>
    {
        Task<AreaInfoEntity> GetAreaInfoByName(string name);
    }
}
