using System;
using System.Collections.Generic;
using System.Text;
using MyWebApiProject.IRepository;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Repository.Base;
namespace MyWebApiProject.Repository
{
   public class GoodsRepository:BaseRepository<GoodsEntity>,IGoodsRepository
    {
    }
}
