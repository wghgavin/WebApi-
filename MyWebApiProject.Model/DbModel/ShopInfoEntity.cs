using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Model.DbModel
{
   public class ShopInfoEntity
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int AreaId { get; set; }
        public string ShopName { get; set; }

    }
}
