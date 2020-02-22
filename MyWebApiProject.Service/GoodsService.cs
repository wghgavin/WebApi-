using System;
using System.Collections.Generic;
using System.Text;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Service.Base;
using MyWebApiProject.IRepository;
namespace MyWebApiProject.Service
{
    public class GoodsService : BaseService<GoodsEntity>, IGoodsService
    {
        IGoodsRepository dal;
        public GoodsService(IGoodsRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
    }
}
