using MyWebApiProject.IService.Base;
using MyWebApiProject.Model.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.IService
{
   public interface IOrderIdInfoService : IBaseService<OrderIdInfoEntity>
    {
        Task<List<OrderIdInfoEntity>> GetOrderIdByUserName(string UserName);
    }
}
