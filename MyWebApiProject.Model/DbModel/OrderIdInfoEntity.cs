using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Model.DbModel
{
    /// <summary>
    /// 订单与客户关联表
    /// </summary>
    public class OrderIdInfoEntity
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        [SugarColumn(IsNullable = false)]
        public string UserName { get; set; }
        [SugarColumn(IsNullable = false)]
        public string OrderId { get; set; }
    }
}
