using MyWebApiProject.Model.DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Model.ViewModel
{
   public class AreaShopInfo
    {
        /// <summary>
        /// 地区名
        /// </summary>

        public string AreaName { get; set; }

        /// <summary>
        /// 平台号
        /// </summary>
        public string PlatFormCode { get; set; }
        /// <summary>
        /// 拼单数量
        /// </summary>
        public string SharingNum { get; set; }
        /// <summary>
        ///起拼价
        /// </summary>
        public string StartPrice { get; set; }
        /// <summary>
        /// 剩余时间
        /// </summary>
        public decimal LeftTime { get; set; }
        public List<string> shopNames { get; set; }
    }
}
