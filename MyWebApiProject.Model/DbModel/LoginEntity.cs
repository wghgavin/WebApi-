﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Model.DbModel
{
    /// <summary>
    /// 拼单项目登录接口
    /// </summary>
    public class LoginEntity
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        [SugarColumn(IsNullable = false)]
        public string UserName { get; set; }
        [SugarColumn(IsNullable = false)]
        public string PassWord { get; set; }
    }
}
