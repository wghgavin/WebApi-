using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Model.DbModel
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class SysUserInfo
    {
        public SysUserInfo()
        {

        }
        public SysUserInfo(string loginName, string loginPWD)
        {
            UloginName = loginName;
            UloginPWD = loginPWD;
            UrealName = UloginName;
            Ustatus = 0;
            UcreateTime = DateTime.Now;
            UupdateTime = DateTime.Now;
            UlastErrTime = DateTime.Now;
            UerrorCount = 0;
            Name = "";

        }
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int UiD { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string UloginName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string UloginPWD { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string UrealName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Ustatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
        public string Uremark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime UcreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime UupdateTime { get; set; } = DateTime.Now;
        /// <summary>
        ///最后登录时间 
        /// </summary>
        public DateTime UlastErrTime { get; set; } = DateTime.Now;

        /// <summary>
        ///错误次数 
        /// </summary>
        public int UerrorCount { get; set; }



        /// <summary>
        /// 登录账号
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string Name { get; set; }

        // 性别
        [SugarColumn(IsNullable = true)]
        public int Sex { get; set; } = 0;
        // 年龄
        [SugarColumn(IsNullable = true)]
        public int Age { get; set; }
        // 生日
        [SugarColumn(IsNullable = true)]
        public DateTime Birth { get; set; } = DateTime.Now;
        // 地址
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string Addr { get; set; }

        [SugarColumn(IsNullable = true)]
        public bool TdIsDelete { get; set; }


        [SugarColumn(IsIgnore = true)]
        public List<int> RIDs { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<string> RoleNames { get; set; }
    }
}
