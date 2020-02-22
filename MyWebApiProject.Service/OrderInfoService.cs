using System;
using System.Collections.Generic;
using System.Text;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Service.Base;
using MyWebApiProject.IRepository;
namespace MyWebApiProject.Service
{
   public class OrderInfoService:BaseService<OrderInfoEntity>,IOrderInfoService
    {
        IOrderInfoRepository dal;
        public OrderInfoService(IOrderInfoRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
    }
}
