using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Model.DbModel
{
    /// <summary>
    /// 拼单信息表
    /// </summary>
    public class OrderInfoEntity
    {
        /// <summary>
        /// 拼单唯一识别码
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 1:发布成功
        /// 2.拼主取消拼单
        /// 3.拼客取消拼单
        /// 4.拼单过期
        /// 5.拼主已付款
        /// 6.拼客已付款
        /// 7.拼单成功
        /// </summary>
        public int Status { get; set; } = 1;
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 地名
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 平台号
        /// </summary>
        public string PlatFormCode { get; set; }
        /// <summary>
        /// 商家名
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 起拼价格
        /// </summary>
        public string StartPrice { get; set; }
        /// <summary>
        /// 时效(分钟计算)
        /// </summary>

        public decimal CachedTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }

    }
}
