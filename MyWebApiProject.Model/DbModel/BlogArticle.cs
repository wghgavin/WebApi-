using MyWebApiProject.Model.ViewModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Model.DbModel
{

        public class BlogArticle : IMapperTo<BlogViewModels>
        {
            /// <summary>
            /// 主键
            /// </summary>
            [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
            public int bID { get; set; }
            /// <summary>
            /// 创建人
            /// </summary>
            [SugarColumn(ColumnDataType = "nvarchar", Length = 60, IsNullable = true)]
            public string bsubmitter { get; set; }

            /// <summary>
            /// 博客标题
            /// </summary>
            [SugarColumn(ColumnDataType = "nvarchar", Length = 256, IsNullable = true)]
            public string btitle { get; set; }

            /// <summary>
            /// 类别
            /// </summary>
            [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
            public string bcategory { get; set; }

            /// <summary>
            /// 内容
            /// </summary>
            [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
            public string bcontent { get; set; }

            /// <summary>
            /// 访问量
            /// </summary>
            public int btraffic { get; set; }

            /// <summary>
            /// 评论数量
            /// </summary>
            public int bcommentNum { get; set; }

            /// <summary> 
            /// 修改时间
            /// </summary>
            public DateTime bUpdateTime { get; set; }

            /// <summary>
            /// 创建时间
            /// </summary>
            public System.DateTime bCreateTime { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
            public string bRemark { get; set; }
            /// <summary>
            /// 逻辑删除
            /// </summary>
            [SugarColumn(IsNullable = true)]
            public bool? IsDeleted { get; set; }
        }
    
}
