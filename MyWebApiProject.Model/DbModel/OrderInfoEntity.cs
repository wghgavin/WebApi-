using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Model.DbModel
{
   public class OrderInfoEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string MatchCus { get; set; }
        public DateTime OrderTime { get; set; }
    }
}
