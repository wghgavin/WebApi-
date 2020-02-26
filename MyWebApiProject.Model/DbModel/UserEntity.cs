using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Model.DbModel
{
   public class UserEntity
    {
       /// <summary>
       /// 唯一ID号
       /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
    }
}
