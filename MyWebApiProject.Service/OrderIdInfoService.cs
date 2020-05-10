using MyWebApiProject.IRepository;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.Service
{
   public class OrderIdInfoService : BaseService<OrderIdInfoEntity>, IOrderIdInfoService
    {
        IOrderIdInfoRepository _dal;
        public OrderIdInfoService(IOrderIdInfoRepository dal)
        {
            this._dal = dal;
        }

        public async Task<List<OrderIdInfoEntity>> GetOrderIdByUserName(string UserName)
        {
          return await  _dal.Query(obj => obj.UserName == UserName);
        }
    }
}
